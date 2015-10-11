using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

/* 
 * The tournament is a ladder tournament where each participant gets a chance
 * to hill climb to the top. When they lose, their turn ends and the next one starts.
 */
public class Tournament : MonoBehaviour {

    public GameObject cell;
    public float tweenTime;
    private GameObject canvas;
    private List<Participant> participants;
    private List<GameObject> ladder;
    private int nextTurn;
    private bool waiting;

	[Header("Players")]
	public List<ARPSPlayer> aiPlayers;
	[Tooltip("Leave empty for testing the tournament without input")]
	public HumanPlayer humanPlayer;

	//For handling matches async
	private bool asyncMatchEnd = false;
	private int asyncChallenger;
	private int asyncChallengee;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("TournamentCanvas");

		//Prepare the list of players
		aiPlayers.Sort((ARPSPlayer p1, ARPSPlayer p2) => { return p2.difficulty - p1.difficulty; }); //Sort the players by difficulty
		TournamentState.instance.Competitors = aiPlayers;
		if (humanPlayer != null)
		{
			humanPlayer.name = TournamentState.instance.PlayerName; //Set the human player to have the name saved in TournamentState
			TournamentState.instance.Competitors.Add(humanPlayer);
		}

        ladder = CreateLadder(TournamentState.instance.Competitors);
        participants = ladder.Select(x => x.GetComponent<Participant>()).Reverse().ToList();
        nextTurn = 0;
        waiting = false;
        Application.LoadLevelAdditive("battle");
	}
	
	// Update is called once per frame
	void Update () {
	    if (waiting == false) {
            waiting = true;
            var challengerPos = ladder.FindIndex(x => x.GetComponent<Participant>() == participants[nextTurn]);
            var ordered = ladder.Select(x => x.GetComponent<Participant>()).ToList();
            participants[nextTurn].challengeOne(ordered, 
                (int challengeePos) => {
					SetupMatch(challengerPos, challengeePos);
                }
            );
        } else if(asyncMatchEnd)
		{
			asyncMatchEnd = false;
			EndMatch(asyncChallenger, asyncChallengee);
		}
	}

	//Not needed / only for testing
    List<ARPSPlayer> CreateScriptableObjects(int amount)
    {
        var list = new List<ARPSPlayer>();
        for (var i = 0; i < amount - 1; i++)
        {
            list.Add(ScriptableObject.CreateInstance<RandomAI>());
            list[i].name = "RandomAI " + (i + 1).ToString();
        }
        list.Add(ScriptableObject.CreateInstance<HumanPlayer>());
        //list[amount - 1].name = "Player";
        return list;
    }

    void SetupMatch(int challenger, int challengee)
    {
        if (challenger == challengee) //Challenger has forfeited their turn
        {
            IncrementTurn();
            waiting = false;
        }
		else
		{
			var chrAvatar = ladder[challenger].GetComponent<Participant>().avatar;
			var cheeAvatar = ladder[challengee].GetComponent<Participant>().avatar;
			ladder[challenger].transform.DOScale(1.2f, tweenTime);
			ladder[challengee].transform.DOScale(1.2f, tweenTime).OnComplete(() => {
				new Match(chrAvatar, cheeAvatar, (Match match) =>
				{
					TournamentState.instance.Matches.Add(match);
					asyncMatchEnd = true;
					asyncChallenger = challenger;
					asyncChallengee = challengee;
				});
			});
		}
    }

    void EndMatch(int challenger, int challengee)
    {
        ladder[challenger].transform.DOScale(1f, tweenTime);
        ladder[challengee].transform.DOScale(1f, tweenTime).OnComplete(() =>
        {
            waiting = false;
        });

		Match m = TournamentState.instance.Matches.Last();
        if (m.getLastWinner() == m.playerOne)
        {
            HandleChallengerVictory(challenger, challengee);
        }
        else
        {
            IncrementTurn();
        }
    }

    void HandleChallengerVictory(int winnerPos, int loserPos)
    {
        if (winnerPos > loserPos)
        {
            var p = ladder[winnerPos];
            ladder.RemoveAt(winnerPos);
            ladder.Insert(loserPos, p);
            RepositionCells();
        }
    }

    void RepositionCells()
    {
        var nextY = -30 + ladder.Count / 2 * 64;
        for (var i = 1; i <= ladder.Count; i++)
        {
            var p = ladder[i - 1];
            p.transform.GetComponentsInChildren<Text>()[1].text = i.ToString();
            if (p.transform.position.y > nextY + 1)
            {
                p.transform.DOMove(new Vector3(0, nextY, 0), tweenTime)
                .OnComplete(() => p.transform.DOPunchPosition(Vector3.up * 20, tweenTime));
            }
            else if (p.transform.position.y < nextY - 1)
            {
                p.transform.DOMove(new Vector3(0, nextY, 0), tweenTime)
                .OnComplete(() => p.transform.DOPunchPosition(Vector3.down * 20, tweenTime));
            }          
            nextY -= 64;
        }
    }

    void IncrementTurn()
    {
        nextTurn = (nextTurn + 1) % participants.Count;
    }

    List<GameObject> CreateLadder(List<ARPSPlayer> players)
    {
        var list = new List<GameObject>();
        var next_y = -30 + players.Count / 2 * 64;
        for (var i = 0; i < players.Count; i++)
        {
            var obj = (GameObject)Instantiate(cell, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(canvas.transform, false);
            obj.transform.GetComponentsInChildren<Text>()[0].text = players[i].name;
            obj.transform.GetComponentsInChildren<Text>()[1].text = (i + 1).ToString();
            obj.transform.position = new Vector3(0, next_y, 0);
            obj.transform.GetComponent<Participant>().avatar = players[i];
            list.Add(obj);

            next_y -= 64;
        }
        return list;
    }
}
