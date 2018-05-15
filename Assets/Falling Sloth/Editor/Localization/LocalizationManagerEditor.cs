using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace FallingSloth.Localization
{
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            if (GUILayout.Button("Open Localization Window"))
                EditorWindow.GetWindow<LocalizationWindow>().Show();

            base.OnInspectorGUI();
        }
    }
}
