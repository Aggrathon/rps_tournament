using System.Collections.Generic;

public class Match {

	public enum RPS
	{
		Rock = 0,
		Paper = 1,
		Scissors = 2
	}

	private IRPSPlayer player1;
	private IRPSPlayer player2;

	private bool player1HasPlayed;
	private bool player2HasPlayed;

	private LinkedList<RPS> player1Choices;
	private LinkedList<RPS> player2Choices;


	public Match(IRPSPlayer player1, IRPSPlayer player2)
	{
		this.player1 = player1;
		this.player2 = player2;

		player1Choices = new LinkedList<RPS>();
		player2Choices = new LinkedList<RPS>();

		newRound();
	}

	public IRPSPlayer playerOne { get { return player1; } }
	public IRPSPlayer playerTwo { get { return player2; } }

	public LinkedList<RPS> playerOneChoices { get { return player1Choices; } }
	public LinkedList<RPS> playerTwoChoices { get { return player2Choices; } }

	public RPS getLastChoiceOther(IRPSPlayer player)
	{
		if(player == player1)
		{
			return player2Choices.Last.Value;
		}
		else
		{
			return player1Choices.Last.Value;
		}
	}

	private void newRound()
	{
		player1HasPlayed = false;
		player2HasPlayed = false;

		player1Choices.AddLast(RPS.Rock);
		player2Choices.AddLast(RPS.Rock);

		player1.newTurn();
		player2.newTurn();
	}

	public void setPlayerChoice(IRPSPlayer player, RPS choice)
	{
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
			//player 1 has won
		}
		else if (result == 2)
		{
			//player 2 has won
		}
	}


}
