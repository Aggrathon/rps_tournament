using UnityEngine;
using System.Collections.Generic;
using System;

public class Participant : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void challengeOne(List<Participant> ladder, Action<int> callback)
    {
        //TODO
        callback(Math.Max(0, ladder.FindIndex(x => x == this) - 1));
    }
}
