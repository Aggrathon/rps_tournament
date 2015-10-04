using UnityEngine;

public class HumanPlayer : ARPSPlayer
{
	private BattleUI ui;

	private delegate void next();
	private delegate void open(HumanPlayer hp, ARPSPlayer ap, Match m);

	public HumanPlayer()
	{
		name = "Human Player";
		health = 5;
		catchPhrase = "";
		winPhrase = "";
		loosePhrase = "";
	}

	public HumanPlayer(string name, Sprite look)
	{
		this.name = name;
		this.look = look;
		health = 5;
		catchPhrase = "";
		winPhrase = "";
		loosePhrase = "";
	}

	public override void startMatch(Match match)
	{
		base.startMatch(match);
		ui = GameObject.FindObjectOfType<BattleUI>();
		ui.Show(this);
	}

	public override void newRound()
	{
		ui.NewRound();
	}

	public override void endMatch()
	{
		ui.Hide();
	}

	public void selectChoice(Match.RPS choice)
	{
		if(currentMatch != null)
		{
			currentMatch.setPlayerChoice(this, choice);
		}
	}

	public void setFinishedClosing()
	{
		currentMatch.setPlayerFinished(this);
	}

	public Match getMatch()
	{
		return currentMatch;
	}
}
