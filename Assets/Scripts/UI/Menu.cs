using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour {

	public InputField playerName;

	public void StartLadderTournament()
	{
		if(playerName.text != "")
		{
			TournamentState.instance.PlayerName = playerName.text;
		}
		Application.LoadLevel("tournament");
	}

	public void OpenMenu()
	{
		Application.LoadLevel("menu");
	}

	public void QuitApplication()
	{
		Application.Quit();
	}
}
