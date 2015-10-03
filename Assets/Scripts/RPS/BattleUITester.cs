using UnityEngine;
using UnityEditor;

public class BattleUITester : MonoBehaviour {

	public HumanPlayer humanPlayer;
	public ARPSPlayer aiPlayer;
	
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
			if(EditorApplication.isPlaying == false)
			{
				EditorApplication.isPlaying = true;
			}
			BattleUITester curr = (this.target as BattleUITester);
			new Match(curr.humanPlayer, curr.aiPlayer, (Match match)=> { });
		}
	}
}
