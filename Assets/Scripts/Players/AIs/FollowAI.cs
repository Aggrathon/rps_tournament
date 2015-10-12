using UnityEngine;

/// <summary>
/// This AI makes a choice based on the other player's last choice
/// </summary>
public class FollowAI : ARPSPlayer
{
	public enum Offset
	{
		SameAsEnemyLast,
		BeatingEnemyLast,
		LoosingToEnemyLast
	}

	[Header("AI Options")]
	public Offset followMode;

	public override void startMatch(Match match)
	{
		base.startMatch(match);
	}

	public override void newRound()
	{
		Match.RPS last = currentMatch.getLastChoiceOther(this);
		if(last == Match.RPS.UNDEFINED)
		{
			IRPSPlayer other = currentMatch.getOtherPlayer(this);
			Match l = TournamentState.instance.Matches.FindLast(
				(Match m) => { return m.playerOne == other || m.playerTwo == other; });
			if(l != null)
			{
				last = l.getLastChoicePlayer(other);
			}
		}
		setChoice(last);
	}

	private void setChoice(Match.RPS otherLast)
	{
		if(otherLast == Match.RPS.UNDEFINED)
		{
			otherLast = (Match.RPS)(new System.Random().Next() % 3);
		}

		if(followMode == Offset.LoosingToEnemyLast)
		{
			int c = (int)otherLast - 1;
			if (c == -1) c = 2;
			otherLast = (Match.RPS)c;
		}
		else if(followMode == Offset.BeatingEnemyLast)
		{
			int c = (int)otherLast + 1;
			if (c == 3) c = 0;
			otherLast = (Match.RPS)c;
		}

		currentMatch.setPlayerChoice(this, otherLast);
	}

	public override void endMatch()
	{
		base.endMatch();
	}
}
