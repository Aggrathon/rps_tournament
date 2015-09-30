using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class Tournament : MonoBehaviour {

    public GameObject cell;
    GameObject canvas;
    List<Participant> participants;
    List<GameObject> ladder;
    int nextTurn;
    float timeUntilNext;
    bool waiting;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas");
        ladder = Create(16);
        participants = ladder.Select(x => x.GetComponent<Participant>()).Reverse().ToList();
        nextTurn = 0;
        timeUntilNext = 0;
        waiting = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if (waiting == false && timeUntilNext <= 0) {
            var challengerPos = ladder.FindIndex(x => x.GetComponent<Participant>() == participants[nextTurn]);
            var ordered = ladder.Select(x => x.GetComponent<Participant>()).ToList();
            participants[nextTurn].challengeOne(ordered, 
                (int challengeePos) => {
                    SetupMatch(challengerPos, challengeePos);
                }
            );
            waiting = true;
            IncrementNext();
        }
        else {
            timeUntilNext -= Time.deltaTime;
        }
	}

    void IncrementNext()
    {
        nextTurn = (nextTurn + 1) % participants.Count;
    }

    void SetupMatch(int challenger, int challengee)
    {
        //TODO
        ladder[challenger].transform.DOScale(1.2f, 1);
        ladder[challengee].transform.DOScale(1.2f, 1).OnComplete(() =>
        {
            ladder[challenger].transform.DOScale(1f, 1);
            ladder[challengee].transform.DOScale(1f, 1).OnComplete(() =>
            {
                timeUntilNext = 1;
                waiting = false;
            });
            HandleChallengerVictory(challenger, challengee);
        });
    }

    List<GameObject> Create(int player_amount)
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
}
