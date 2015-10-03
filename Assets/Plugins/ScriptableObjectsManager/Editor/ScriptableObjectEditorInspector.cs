using UnityEngine;
using UnityEditor;
using System;

namespace aggrathon.ScriptableObjectManager
{

    [InitializeOnLoad]
    [CustomEditor(typeof(ScriptableObject), true)]
    public class ScriptableObjectEditorInspector : Editor
    {
        private static bool SOManager = false;
        static ScriptableObjectEditorInspector()
        {
            if (Type.GetType("aggrathon.ScriptableObjectManager.ScriptableObjectManagerWindow") != null)
                SOManager = true;
        }

        override public void OnInspectorGUI()
        {
            if (SOManager)
            {
                if (GUILayout.Button("Open Scriptable Object Manager"))
                {
                    ScriptableObjectManagerWindow.OpenManagerWindow();
                }
                GUILayout.Space(10);
            }
            DrawDefaultInspector();
        }
    }

    [InitializeOnLoad]
    [CustomEditor(typeof(MonoScript), true)]
    public class ScriptableObjectScriptInspector : Editor
    {
        private static bool SOManager = false;
        static ScriptableObjectScriptInspector()
        {
            if (Type.GetType("aggrathon.ScriptableObjectManager.ScriptableObjectManagerWindow") != null)
                SOManager = true;
        }

        override public void OnInspectorGUI()
        {
            Type script = ((MonoScript)target).GetClass();
            if (script != null && script.IsSubclassOf(typeof(ScriptableObject)) &&
                     !script.IsSubclassOf(typeof(Editor)) &&
                     !script.IsSubclassOf(typeof(EditorWindow)))
            {
                if (GUILayout.Button("Create Scriptable Object"))
                {
                    ScriptableObjectCreator.CreateScriptableObject(script);
                }
                if (SOManager)
                {
                    if (GUILayout.Button("Create Scriptable Object (Options)"))
                    {
                        ScriptableObjectCreatorWindow.ShowCreator(script);
                    }
                }
                if (GUILayout.Button("Open Scriptable Object Manager"))
                {
                    ScriptableObjectManagerWindow.OpenManagerWindow();
                }
                GUILayout.Space(10);
            }
            DrawDefaultInspector();
        }
    }

}
