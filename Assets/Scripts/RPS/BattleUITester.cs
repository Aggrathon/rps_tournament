using UnityEngine;
using UnityEditor;
using System.Collections;

public class BattleUITester : MonoBehaviour {

	public HumanPlayer humanPlayer;
	public ARPSPlayer aiPlayer;

	public bool startMatchOnPlay = false;

	public void Battle()
	{
		new Match(humanPlayer, aiPlayer, (Match match) => { });
	}

	void Start()
	{
		if(startMatchOnPlay && Application.loadedLevelName.Equals("battle"))
		{
			Battle();
		}
	}
}

[InitializeOnLoad]
[CustomEditor(typeof(BattleUITester))]
public class BUITInspector : Editor
{

	override public void OnInspectorGUI()
	{
		DrawDefaultInspector();
		GUILayout.Space(10);
		if (GUILayout.Button("Run Test Match"))
		{
			if (EditorApplication.isPlaying == false)
			{
                EditorApplication.isPlaying = true;
				(this.target as BattleUITester).startMatchOnPlay = true;
			}
			else
			{
				(this.target as BattleUITester).Battle();
			}
		}
	}
}

