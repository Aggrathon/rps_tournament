using UnityEngine;

public class RandomAI : ARPSPlayer
{

	public override void newRound()
	{
		if(currentMatch != null)
		{
			currentMatch.setPlayerChoice(this, (Match.RPS)Random.Range(0, 2));
		}
	}
}
