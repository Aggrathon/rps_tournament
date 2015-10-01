using UnityEngine;
using System.Collections;
using System;

public abstract class ARPSPlayer : ScriptableObject, IRPSPlayer {

	protected Match currentMatch;
	private int currentHealth;

	[Header("Player Attributes")]
	new public string name;
	public Sprite look;
	public string catchPhrase;
	public string winPhrase;
	public string loosePhrase;
	public int maxHealth = 5;

	public int health { get { return currentHealth; } set { currentHealth = value; } }

	/// <summary>
	/// This method sets up everything needed for a match
	/// Call this version of the method FIRST if overwritten
	/// </summary>
	/// <param name="match"></param>
	virtual public void startMatch(Match match)
	{
		currentMatch = match;
		currentHealth = maxHealth;
	}

	/// <summary>
	/// This method cleans up after a match is over
	/// Call this version of the method LAST if overwritten
	/// </summary>
	virtual public void endMatch()
	{
		Match oldm = currentMatch;
		currentMatch = null;
		oldm.setPlayerFinished(this);
	}

	/// <summary>
	/// This method informs the player that a choice/move is needed
	/// The choice should then be registered with 'currentMatch.setPlayerChoice(this, choice);
	/// </summary>
	public abstract void newRound();
}
