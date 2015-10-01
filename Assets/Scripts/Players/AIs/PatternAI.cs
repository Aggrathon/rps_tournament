using UnityEngine;

public class PatternAI : ARPSPlayer
{

	[Header("AI Config")]
	public Match.RPS[] pattern;
	public bool resetEachMatch;

	private int index = 0;

	public override void startMatch(Match match)
	{
		base.startMatch(match);
		if(resetEachMatch)
		{
			index = 0;
		}
	}

	public override void newRound()
	{
		currentMatch.setPlayerChoice(this, pattern[index]);
		index = (index + 1) % pattern.Length;
	}
}
