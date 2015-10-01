using UnityEditor;
using UnityEngine;

public class MatchAndPlayerTester : ScriptableObject {
	public ARPSPlayer[] players;
}



[InitializeOnLoad]
[CustomEditor(typeof(MatchAndPlayerTester), true)]
public class TestMatchInspector : Editor
{

	override public void OnInspectorGUI()
	{
		if (GUILayout.Button("Run Test Matches on Players"))
		{
			ARPSPlayer[] players = (target as MatchAndPlayerTester).players;
            for (int i = 0; i < players.Length; i++)
			{
				for(int j = i+1; j < players.Length; j++)
				{
					new Match(players[i], players[j], (Match m) =>
					{
						Debug.Log("Match between " + (m.playerOne as ARPSPlayer).name + " and " + (m.playerTwo as ARPSPlayer).name + " finished in " + m.playerOneChoices.Count + " rounds");
						if(m.playerOne.health > 0)
						{
							Debug.Log("Winner: "+(m.playerOne as ARPSPlayer).name+" ("+m.playerOne.health+" lives left)");
						} else
						{
							Debug.Log("Winner: " + (m.playerTwo as ARPSPlayer).name + " (" + m.playerTwo.health + " lives left)");
						}
					});
				}
			}
		}
		GUILayout.Space(10);
		DrawDefaultInspector();
	}
}
