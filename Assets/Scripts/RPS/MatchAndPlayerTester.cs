using UnityEditor;
using UnityEngine;

public class MatchAndPlayerTester : ScriptableObject {
	public ARPSPlayer[] players;
}



[InitializeOnLoad]
[CustomEditor(typeof(MatchAndPlayerTester), true)]
public class TestMatchInspector : Editor
{
	int i, j;


	override public void OnInspectorGUI()
	{
		if (GUILayout.Button("Run Test Matches on Players"))
		{
			Debug.Log("Test started");
			i = 0;
			j = 1;
			startMatch();
		}
		GUILayout.Space(10);
		DrawDefaultInspector();
	}

	public void startMatch()
	{
		ARPSPlayer[] players = (target as MatchAndPlayerTester).players;
		new Match(players[i], players[j], (Match m) =>
		{
			string result = "Match between " + (m.playerOne as ARPSPlayer).name + " and " + (m.playerTwo as ARPSPlayer).name
				+ " finished in " + m.playerOneChoices.Count + " rounds \n";
            if (m.playerOne.health > 0)
			{
				result += "Winner: " + (m.playerOne as ARPSPlayer).name + "(" + m.playerOne.health + " lives left)";
            }
			else
			{
				result += "Winner: " + (m.playerTwo as ARPSPlayer).name + " (" + m.playerTwo.health + " lives left)";
			}
			Debug.Log(result);

			j++;
			if (j >= players.Length)
			{
				i++;
				if (i >= players.Length -1)
				{
					Debug.Log("Test completed");
					return;
                }
				j = i + 1;
			}
			startMatch();
		});
	}
}
