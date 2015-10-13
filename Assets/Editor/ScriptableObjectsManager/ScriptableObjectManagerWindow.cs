using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

namespace aggrathon.ScriptableObjectManager {

	public class ScriptableObjectManagerWindow : EditorWindow {
		
		private Vector2 typeScroll;
		private Vector2 objectScroll;
		private MonoScript[] types;
		private ScriptableObject[] objects;
		private Dictionary<Type, Boolean> showing;

		public ScriptableObjectManagerWindow() {
			showing = new Dictionary<Type, bool>();
		}

		[MenuItem("Window/Scriptable Object Manager")]
		public static void OpenManagerWindow() {
			EditorWindow.GetWindow<ScriptableObjectManagerWindow>("SO Manager");
		}

		/// <summary>
		/// Repopulates the lists of scripts and objects
		/// </summary>
        public void Refresh()
        {
            MonoScript[] scripts = (from t in AssetDatabase.FindAssets("t:Script")
                                    select (MonoScript)AssetDatabase.LoadAssetAtPath(
                AssetDatabase.GUIDToAssetPath(t), typeof(MonoScript)) ).ToArray();
			types = (from s in scripts where s != null && s.GetClass() != null &&
			         s.GetClass().IsSubclassOf(typeof(ScriptableObject))&&
			         !s.GetClass().IsSubclassOf(typeof(Editor))&&
			         !s.GetClass().IsSubclassOf(typeof(EditorWindow))&&
					 !s.GetClass().IsAbstract
			         select s).ToArray();
			foreach(MonoScript ms in types) {
				if(!showing.ContainsKey(ms.GetClass()))
					showing.Add(ms.GetClass(), true);
			}

			objects = (from t in AssetDatabase.FindAssets("t:ScriptableObject")
			           select (ScriptableObject)AssetDatabase.LoadAssetAtPath(
				AssetDatabase.GUIDToAssetPath(t), typeof(ScriptableObject))
			           ).ToArray();
		}
		
		void OnFocus() {
			Refresh();
		}

		/// <summary>
		/// Draws the Manager window.
		/// </summary>
		void OnGUI () {
			GUIStyle panel = new GUIStyle("Window");
			panel.fontStyle = FontStyle.Bold;
			GUIStyle leftAlign = new GUIStyle("Label");
			leftAlign.alignment = TextAnchor.MiddleLeft;
			GUIStyle boldLabel = new GUIStyle("Label");
			boldLabel.alignment = TextAnchor.MiddleLeft;
			boldLabel.fontStyle = FontStyle.Bold;

			GUILayout.BeginHorizontal();
			
			GUILayout.BeginVertical(GUILayout.MaxWidth(this.position.width/2));

			//First Panel
			GUILayout.BeginVertical("Scriptable Object Types", panel);
			
			//TypeList
			typeScroll = GUILayout.BeginScrollView(typeScroll);
			foreach(MonoScript ms in types) {
				if(ms == null || ms.GetClass() == null)
					continue;
				GUILayout.BeginHorizontal("Box");


				bool af, bef = true;
				if(!showing.TryGetValue(ms.GetClass(), out bef)) {
					showing.Add(ms.GetClass(), bef);
				}
				af = bef;
				af = GUILayout.Toggle(bef, new GUIContent(System.String.Empty, "Show/Hide objects of this type in the list"), GUILayout.ExpandWidth(false));
				if(af != bef)
					showing[ms.GetClass()] = af;
				
				GUIStyle style = (Selection.activeObject == ms? boldLabel : leftAlign);
				if(GUILayout.Button(new GUIContent(ms.GetClass().Name, "Click to Select"), style, GUILayout.ExpandWidth(true))) {
					Selection.activeObject = ms;
				}
				if(GUILayout.Button(new GUIContent("+", "Create Object"), GUILayout.ExpandWidth(false))) {
					ScriptableObjectCreator.CreateScriptableObject(ms.GetClass());
					return;
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();

			GUILayout.BeginHorizontal();
			if(GUILayout.Button("Select all")) {
				var keys = new List<Type>(showing.Keys);
				foreach (Type key in keys)
				{
					showing[key] = true;
				}
			}
			if(GUILayout.Button("Select none")) {
				var keys = new List<Type>(showing.Keys);
				foreach (Type key in keys)
				{
					showing[key] = false;
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();

			if(GUILayout.Button("Reverse sorting")) {
				Array.Reverse(types);
				Array.Reverse(objects);
			}
			if(GUILayout.Button("Create new script")) {
				ScriptableObjectCreator.ShowScriptCreatorWindow();
			}
			if(GUILayout.Button("Refresh")) {
				Refresh();
			}
			GUILayout.EndVertical();

			//Second Panel
			GUILayout.BeginVertical("Scriptable Object Instances", panel);
			
			//ObjectList
			objectScroll = GUILayout.BeginScrollView(objectScroll);
			foreach(ScriptableObject s in objects) {
				if(s==null)
					continue;
				bool show = true;
				if(showing.TryGetValue(s.GetType(), out show)) {
					if(!show)
						continue;
				}

				GUILayout.BeginHorizontal("Box");
				GUIStyle style = (Selection.activeObject == s? boldLabel : leftAlign);
				if(GUILayout.Button(new GUIContent(s.name, "Click to Select"), style)) {
					Selection.activeObject = s;
				}
				if(GUILayout.Button(new GUIContent("C", "Clone"), GUILayout.ExpandWidth(false))) {
					string path = AssetDatabase.GetAssetPath(s);
					string newPath = AssetDatabase.GenerateUniqueAssetPath(path);
					AssetDatabase.CopyAsset(path, newPath);
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
					Refresh();
				}
				if(GUILayout.Button(new GUIContent("–", "Delete"), GUILayout.ExpandWidth(false))) {
					AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(s));
					AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
					Refresh();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();

			GUILayout.EndVertical();

			GUILayout.EndHorizontal();
		}
	}

}

