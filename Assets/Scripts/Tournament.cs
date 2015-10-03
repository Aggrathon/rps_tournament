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

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
        ladder = CreateLadder(16);
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

    List<GameObject> CreateLadder(int player_amount)
    {
        var list = new List<GameObject>();
        var next_y = -30 + player_amount / 2 * 64;
        for (var i = 1; i <= player_amount; i++)
        {
            var obj = (GameObject)Instantiate(cell, Vector3.zero, Quaternion.identity);
            obj.transform.SetParent(canvas.transform, false);
            obj.transform.GetComponentsInChildren<Text>()[0].text = "Player " + (player_amount + 1 - i).ToString();
            obj.transform.GetComponentsInChildren<Text>()[1].text = i.ToString();
            obj.transform.position = new Vector3(0, next_y, 0);
            list.Add(obj);

            next_y -= 64;
        }
        return list;
    }
}
