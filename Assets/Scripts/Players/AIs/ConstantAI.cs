using UnityEngine;

public class ConstantAI : ARPSPlayer {

	[Header("AI Config")]
	public Match.RPS constantChoice;

	public override void newRound()
	{
		if(currentMatch != null)
			currentMatch.setPlayerChoice(this, constantChoice);
	}
}
