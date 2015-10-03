using UnityEngine;

public class HumanPlayer : ARPSPlayer
{
	private BattleUI ui;

	public HumanPlayer()
	{
		name = "Human Player";
		health = 5;
		catchPhrase = "";
		winPhrase = "";
		loosePhrase = "";
	}

	public HumanPlayer(string name)
	{
		this.name = name;
		health = 5;
		catchPhrase = "";
		winPhrase = "";
		loosePhrase = "";
	}

	public override void startMatch(Match match)
	{
		base.startMatch(match);
		ui = GameObject.FindObjectOfType<BattleUI>();
	}

	public override void newRound()
	{
	}

	public override void endMatch()
	{
		base.endMatch();
	}

	public void selectChoice(Match.RPS choice)
	{

	}
}
