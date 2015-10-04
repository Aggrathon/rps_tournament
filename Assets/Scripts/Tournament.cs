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
    private List<ARPSPlayer> competitors;
    private List<Participant> participants;
    private List<GameObject> ladder;
    //private List<ARPSPlayer> players;
    private int nextTurn;
    private bool waiting;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
        TournamentState.instance.Competitors = CreateScriptableObjects(16);
        competitors = TournamentState.instance.Competitors;
        ladder = CreateLadder(competitors);
        participants = ladder.Select(x => x.GetComponent<Participant>()).Reverse().ToList();
        nextTurn = 0;
        waiting = false;
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
        }
	}

    List<ARPSPlayer> CreateScriptableObjects(int amount)
    {
        var list = new List<ARPSPlayer>();
        for (var i = 0; i < amount; i++)
        {
            list.Add(ScriptableObject.CreateInstance<RandomAI>());
            list[i].name = "RandomAI " + (i + 1).ToString();
        }
        return list;
    }

    void SetupMatch(int challenger, int challengee)
    {
        if (challenger == challengee) //Challenger has forfeited their turn
        {
            IncrementTurn();
            waiting = false;
            return;
        }

        ladder[challenger].transform.DOScale(1.2f, tweenTime);
        ladder[challengee].transform.DOScale(1.2f, tweenTime).OnComplete(() =>
        {     
            //ACTUAL BATTLE STUFF GOES HERE, ASYNC FUNCTION WHOSE CALLBACK CALLS EndMatch
            var challengerWon = Random.value < 0.5 ? true : false;
            EndMatch(challenger, challengee, challengerWon);
        });
    }

    void EndMatch(int challenger, int challengee, bool challengerWon)
    {
        ladder[challenger].transform.DOScale(1f, tweenTime);
        ladder[challengee].transform.DOScale(1f, tweenTime).OnComplete(() =>
        {
            waiting = false;
        });

        if (challengerWon)
        {
            HandleChallengerVictory(challenger, challengee);
        }
        else
        {
            IncrementTurn();
        }
    }

    void EndMatch(int challenger, int challengee, Match match)
    {
        ladder[challenger].transform.DOScale(1f, tweenTime);
        ladder[challengee].transform.DOScale(1f, tweenTime).OnComplete(() =>
        {
            waiting = false;
        });

        TournamentState.instance.Matches.Add(match);

        if (match.playerOneHealth > match.playerTwoHealth)
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
        var next_y = -30 + ladder.Count / 2 * 64;
        for (var i = 1; i <= ladder.Count; i++)
        {
            var p = ladder[i - 1];
            p.transform.GetComponentsInChildren<Text>()[1].text = i.ToString();
            p.transform.position = new Vector3(0, next_y, 0);

            next_y -= 64;
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
            list.Add(obj);

            next_y -= 64;
        }
        return list;
    }
}
