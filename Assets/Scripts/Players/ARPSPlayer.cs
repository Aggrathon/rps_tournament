using UnityEngine;
using System.Collections;
using System;

public abstract class ARPSPlayer : ScriptableObject, IRPSPlayer {

	protected Match currentMatch;

	[Header("Player Attributes")]
	new public string name;
	public Sprite look;
	public string catchPhrase;
	public string winPhrase;
	public string loosePhrase;
	[SerializeField] private int Health = 3;
	[Range(0, 100)]
	public int difficulty = 50;

	public int health { get { return Health; } set { Health = value; } }

	/// <summary>
	/// This method sets up everything needed for a match
	/// Call this version of the method FIRST if overwritten
	/// </summary>
	/// <param name="match"></param>
	virtual public void startMatch(Match match)
	{
		currentMatch = match;
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
