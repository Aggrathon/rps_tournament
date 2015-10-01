using UnityEngine;
using System.Collections;
using System;

public abstract class ARPSPlayer : ScriptableObject, IRPSPlayer {

	[Header("Player Attributes")]
	public string name;
	public Sprite look;
	public string catchPhrase;
	public string winPhrase;
	public string loosePhrase;

	public abstract void startMatch();
	public abstract void endMatch();
	public abstract void newTurn();
}
