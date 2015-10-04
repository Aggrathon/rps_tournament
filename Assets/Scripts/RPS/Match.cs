using System.Collections.Generic;
using UnityEngine;

public class Match {

	public enum RPS
	{
		Rock = 0,
		Paper = 1,
		Scissors = 2
	}

	public delegate void MatchEnded(Match match);
	private delegate void next();

	private IRPSPlayer player1;
	private IRPSPlayer player2;

	private bool player1HasPlayed;
	private bool player2HasPlayed;

	private LinkedList<RPS> player1Choices;
	private LinkedList<RPS> player2Choices;

	private MatchEnded callback;
	private bool matchHasEnded = false;

	private bool player1HasFinished = false;
	private bool player2HasFinished = false;

	/// <summary>
	/// This creates a match and starts it immediately
	/// </summary>
	/// <param name="player1"></param>
	/// <param name="player2"></param>
	/// <param name="callback">The function called when the match is over</param>
	public Match(IRPSPlayer player1, IRPSPlayer player2, MatchEnded callback)
	{
		this.player1 = player1;
		this.player2 = player2;

        playerOneHealth = player1.health;
        playerTwoHealth = player2.health;

		player1Choices = new LinkedList<RPS>();
		player2Choices = new LinkedList<RPS>();

		this.callback = callback;
		
		player1.startMatch(this);
		player2.startMatch(this);

		newRound();
	}

    public int playerOneHealth { get; private set; }
    public int playerTwoHealth { get; private set; }

	public IRPSPlayer playerOne { get { return player1; } }
	public IRPSPlayer playerTwo { get { return player2; } }

	public LinkedList<RPS> playerOneChoices { get { return player1Choices; } }
	public LinkedList<RPS> playerTwoChoices { get { return player2Choices; } }

	/// <summary>
	/// Get the last choice of the player
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public RPS getLastChoicePlayer(IRPSPlayer player)
	{
		LinkedList<RPS> list = (player1 == player ? player1Choices : player2Choices);
		if (matchHasEnded)
		{
			return list.Last.Value;
		}
		else if (list.Count > 1)
		{
			return list.Last.Previous.Value;
		}
		else
		{
			return (RPS)Random.Range(0, 2);
		}
	}

	/// <summary>
	/// Get the last choice of the other player
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public RPS getLastChoiceOther(IRPSPlayer player)
	{
		return getLastChoicePlayer(getOtherPlayer(player));
	}

	/// <summary>
	/// Returns the other player
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public IRPSPlayer getOtherPlayer(IRPSPlayer player)
	{
		return (player == player2 ? player1 : player2);
	}

	private void newRound()
	{
		player1HasPlayed = false;
		player2HasPlayed = false;

		player1Choices.AddLast(RPS.Rock);
		player2Choices.AddLast(RPS.Rock);

		next del = player1.newRound;
		del.BeginInvoke(null, null);
		del = player2.newRound;
		del.BeginInvoke(null, null);
	}

	/// <summary>
	/// Sets the choice of the player for this round (next round begins when both players have selected their choice)
	/// </summary>
	/// <param name="player"></param>
	/// <param name="choice"></param>
	public void setPlayerChoice(IRPSPlayer player, RPS choice)
	{
		if(matchHasEnded)
		{
			return;
		}
		if(player == player1)
		{
			player1Choices.Last.Value = choice;
			player1HasPlayed = true;
			if(player2HasPlayed)
			{
				endRound();
			}
		}
		else if(player == player2)
		{
			player2Choices.Last.Value = choice;
			player2HasPlayed = true;
			if (player1HasPlayed)
			{
				endRound();
			}
		}
	}

	private void endRound()
	{
		int result = player1Choices.Last.Value - player2Choices.Last.Value;
		if(result < 0)
		{
			result += 3;
		}
		if(result == 1)
		{
			playerTwoHealth--;
			if(playerTwoHealth == 0)
			{
				endMatch();
				return;
			}
		}
		else if (result == 2)
		{
			playerOneHealth--;
			if (playerOneHealth == 0)
			{
				endMatch();
				return;
			}
		}
		newRound();
	}

	private void endMatch()
	{
		if(matchHasEnded)
		{
			return;
		}
		player1.endMatch();
		player2.endMatch();
		player1HasPlayed = false;
		player2HasPlayed = false;
		matchHasEnded = true;
	}

	/// <summary>
	/// Use this to inform the match that the player has finished ending the match (calls the callback method when both players are ready)
	/// </summary>
	/// <param name="player"></param>
	public void setPlayerFinished(IRPSPlayer player)
	{
		if(player1HasFinished && player2HasFinished)
		{
			return;
		}
		if (player == player1)
		{
			player1HasFinished = true;
			if (player2HasFinished)
			{
				matchHasEnded = true;
				callback.BeginInvoke(this, null, null);
			}
		}
		else if (player == player2)
		{
			player2HasFinished = true;
			if (player1HasFinished)
			{
				matchHasEnded = true;
				callback.BeginInvoke(this, null, null);
			}
		}
	}


}
