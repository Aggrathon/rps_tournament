using UnityEngine;
using System.Collections;
using System;

public class FibonacciAI : ARPSPlayer {

	private int num1, num2, count;
	private System.Random rnd = new System.Random();
	private Match.RPS curr = Match.RPS.Rock;

	public override void startMatch(Match match)
	{
		base.startMatch(match);
		num1 = 1;
		num2 = 0;
		count = 0;
	}

	public override void newRound()
	{
		currentMatch.setPlayerChoice(this, curr);
		count++;
		if(count >= num1)
		{
			count = num1;
			num1 = num1 + num2;
			num2 = count;
			count = 0;
			curr = (Match.RPS)(rnd.Next() % 3);
		}
	}
}
