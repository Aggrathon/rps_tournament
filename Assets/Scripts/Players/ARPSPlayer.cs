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

	virtual public void startMatch(Match match)
	{
		currentMatch = match;
		currentHealth = maxHealth;
	}

	virtual public void endMatch()
	{
		currentMatch.setPlayerFinished(this);
		currentMatch = null;
	}

	public abstract void newRound();
}
