using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace FallingSloth.Localization
{
    [CustomEditor(typeof(LanguageData))]
    public class LanguageDataEditor : Editor
    {
        ReorderableList list;

        void OnEnable()
        {
            list = new ReorderableList(serializedObject,
                                       serializedObject.FindProperty("values"),
                                       true, true, true, true);

            list.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, "Values");
            };

            list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width / 2f, EditorGUIUtility.singleLineHeight),
                                        element.FindPropertyRelative("key"),
                                        GUIContent.none);
                EditorGUI.PropertyField(new Rect(rect.x + (rect.width / 2f), rect.y, rect.width / 2f, EditorGUIUtility.singleLineHeight),
                                        element.FindPropertyRelative("value"),
                                        GUIContent.none);
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("language"));

            list.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}