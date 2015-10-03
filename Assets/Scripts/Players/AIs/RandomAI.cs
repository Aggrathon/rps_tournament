using System;

public class RandomAI : ARPSPlayer
{
	private Random rnd;

	public RandomAI()
	{
		rnd = new Random();
	}

	public override void newRound()
	{
		currentMatch.setPlayerChoice(this, (Match.RPS)(rnd.Next()%3));
	}
}
