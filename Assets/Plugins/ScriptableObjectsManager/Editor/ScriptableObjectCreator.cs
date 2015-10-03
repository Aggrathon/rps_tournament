using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;

namespace aggrathon.ScriptableObjectManager {

/// <summary>
/// Handles all the creations of ScriptableObjects
/// Both the creation-code and the Visual elements
/// </summary>
public static class ScriptableObjectCreator {

	/// <summary>
	/// Creates the scriptable object at the selected location
	/// </summary>
	/// <param name="t">Type of ScriptableObject.</param>
	public static void CreateScriptableObject (Type t) {
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == ""){
			Debug.Log("No selection, default location used (asset root).");
			path = "Assets";
		}
		CreateScriptableObject(t, path);
	}

	/// <summary>
	/// Creates the scriptable object at the provided location.
	/// </summary>
	/// <param name="t">Type of ScriptableObject.</param>
	/// <param name="path">Path to save location.</param>
	public static void CreateScriptableObject (Type t, string path) 
	{
		if(!t.IsSubclassOf(typeof(ScriptableObject))) {
			Debug.LogError(t+" is not a SctiptableObject!");
			return;
		}
		if(path == null || path == System.String.Empty) {
			Debug.LogError("No path provided, can't create the ScriptableObject");
			return;
		}

		ScriptableObject asset = ScriptableObject.CreateInstance (t);
		
		if (Path.GetExtension (path) != "") 
		{
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
		}
		
		string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + t.Name + ".asset");
		
		AssetDatabase.CreateAsset (asset, assetPathAndName);
		
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh();
		EditorUtility.FocusProjectWindow ();
		Selection.activeObject = asset;
	}
	

	/// <summary>
	/// Creates a ScriptableObject C# script.
	/// </summary>
	/// <param name="name">Name of the object/script.</param>
	public static void CreateScript (string name) {
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == ""){
			Debug.Log("No selection, default location used (asset root).");
			path = "Assets";
		}
		CreateScript(name, path);
	}

	/// <summary>
	/// Creates a ScriptableObject C# script.
	/// </summary>
	/// <param name="name">Name of the object/script.</param>
	/// <param name="path">Path to save at.</param>
	public static void CreateScript(string name, string path) {
		string filePath = path+"/"+name+".cs";
		if( File.Exists(filePath) == false ){ // do not overwrite
			using (StreamWriter outfile = new StreamWriter(filePath)) {
				outfile.WriteLine("using UnityEngine;");
				outfile.WriteLine("");
				outfile.WriteLine("public class "+name+" : ScriptableObject {");
				outfile.WriteLine("\t");
				outfile.WriteLine("\t");
				outfile.WriteLine("\t");
				outfile.WriteLine("}");
			}//File written
		} else
			Debug.LogError("Script already exists");
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		Selection.activeObject = AssetDatabase.LoadAssetAtPath(filePath, typeof(UnityEngine.Object));
	}
	
	/// <summary>
	/// Shows the SO-creator window
	/// </summary>
	[MenuItem("Assets/Create/ScriptableObject")]
	public static void ShowSOCreatorWindow ()
	{
		ScriptableObjectCreatorWindow.ShowCreator();
	}

	/// <summary>
	/// Shows the scriptcreator window
	/// </summary>
	[MenuItem("Assets/Create/C# ScriptableObject Script")]
	public static void ShowScriptCreatorWindow ()
	{
		ScriptCreatorWindow.ShowCreator();
	}
}

/// <summary>
/// Scriptable object creator window.
/// </summary>
public class ScriptableObjectCreatorWindow : EditorWindow {
	private int selectedObject;
	private bool selectedLocation;
	private string[] names;
	private Type[] types;
	
	public Type[] Types {
		get { return types; }
		set {
			types = value;
			names = types.Select(t => t.Name).ToArray();
		}
	}
	
	public ScriptableObjectCreatorWindow() {
		selectedLocation = true;
	}
	
	/// <summary>
	/// Shows the creator.
	/// </summary>
	public static void ShowCreator() {
		EditorWindow.GetWindow<ScriptableObjectCreatorWindow>(true, "Create a new ScriptableObject", true).ShowPopup();
	}

    /// <summary>
    /// Shows the creator with a preselected script type
    /// </summary>
    /// <param name="selectedScript">The Script to be selected by default</param>
    public static void ShowCreator(Type selectedScript)
    {
        ScriptableObjectCreatorWindow window = EditorWindow.GetWindow<ScriptableObjectCreatorWindow>(true, "Create a new ScriptableObject", true);
        window.ShowPopup();
        window.selectScript(selectedScript);
    }

    public void selectScript(Type scriptType)
    {
        for (int i = 0; i < types.Length; i++) {
            if(types[i].Equals(scriptType)) {
                selectedObject = i;
            }
        }
    }
	
	void OnFocus() {
		MonoScript[] scripts = (from t in AssetDatabase.FindAssets("t:Script")
		                        select (MonoScript)AssetDatabase.LoadAssetAtPath(
			AssetDatabase.GUIDToAssetPath(t), typeof(MonoScript))).ToArray();
		Types = (from t in scripts where 
		         t.GetClass().IsSubclassOf(typeof(ScriptableObject))&&
		         !t.GetClass().IsSubclassOf(typeof(Editor))&&
		         !t.GetClass().IsSubclassOf(typeof(EditorWindow))
		         select t.GetClass()).ToArray();
	}
	
	void OnGUI() {
		GUILayout.Label("Scriptable Object");
		selectedObject = EditorGUILayout.Popup(selectedObject, names);
		selectedLocation = GUILayout.SelectionGrid(
			(selectedLocation?0:1), 
			new string[] { "Selected Location", "Default Location" },
		2 ) == 0;
		if(GUILayout.Button("Create")) {
			if(!selectedLocation)
				Selection.activeObject = null;
			ScriptableObjectCreator.CreateScriptableObject (types[selectedObject]);
			Close ();
		}
	}
}


/// <summary>
/// Scriptable object script creator window.
/// </summary>
public class ScriptCreatorWindow : EditorWindow {
	private bool selectedLocation;
	private string scriptName;

	public ScriptCreatorWindow() {
		selectedLocation = true;
		scriptName = "NewScriptableObject";
	}

	/// <summary>
	/// Shows the creator.
	/// </summary>
	public static void ShowCreator() {
		EditorWindow.GetWindow<ScriptCreatorWindow>(true, "Create a new Script", true).ShowPopup();
	}
	
	void OnGUI() {
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name:",GUILayout.ExpandWidth(false));
		scriptName = GUILayout.TextField(scriptName);
		GUILayout.EndHorizontal();
		selectedLocation = GUILayout.SelectionGrid(
			(selectedLocation?0:1), 
			new string[] { "Selected Location", "Default Location" },
		2 ) == 0;
		if(GUILayout.Button("Create")) {
			if(!selectedLocation)
				Selection.activeObject = null;
			ScriptableObjectCreator.CreateScript (scriptName);
			Close ();
		}
	}
}

}
