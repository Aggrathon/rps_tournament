using UnityEngine;

public class ConstantAI : ARPSPlayer {

	[Header("AI Config")]
	public Match.RPS constantChoice;

	public override void newRound()
	{
		currentMatch.setPlayerChoice(this, constantChoice);
	}
}
