using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace FallingSloth.Audio
{
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : Editor
    {
        ReorderableList list;

        float lineHeight = EditorGUIUtility.singleLineHeight;
        float lineHeightWithSpace = EditorGUIUtility.singleLineHeight + 10f;

        float indent = 20f;

        void OnEnable()
        {
            list = new ReorderableList(serializedObject, serializedObject.FindProperty("sounds"), true, true, true, true);

            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Sounds");
            };

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

                Rect pRect;

                pRect = new Rect(rect.x, rect.y, rect.width, lineHeight);
                EditorGUI.LabelField(pRect, element.FindPropertyRelative("name").stringValue);

                pRect = new Rect(rect.x + indent, rect.y + lineHeightWithSpace, rect.width - indent, lineHeight);
                EditorGUI.PropertyField(pRect, element.FindPropertyRelative("name"));

                pRect = new Rect(rect.x + indent, rect.y + (lineHeightWithSpace * 2f), rect.width - indent, lineHeight);
                EditorGUI.PropertyField(pRect, element.FindPropertyRelative("audioClip"));

                pRect = new Rect(rect.x + indent, rect.y + (lineHeightWithSpace * 3f), rect.width - indent, lineHeight);
                EditorGUI.PropertyField(pRect, element.FindPropertyRelative("playOnAwake"));

                pRect = new Rect(rect.x + indent, rect.y + (lineHeightWithSpace * 4f), rect.width - indent, lineHeight);
                EditorGUI.PropertyField(pRect, element.FindPropertyRelative("loop"));

                pRect = new Rect(rect.x + indent, rect.y + (lineHeightWithSpace * 5f), rect.width - indent, lineHeight);
                EditorGUI.PropertyField(pRect, element.FindPropertyRelative("volume"));

                pRect = new Rect(rect.x + indent, rect.y + (lineHeightWithSpace * 6f), rect.width - indent, lineHeight);
                EditorGUI.PropertyField(pRect, element.FindPropertyRelative("pitch"));
            };

            list.elementHeightCallback = (int index) =>
            {
                return lineHeightWithSpace * 7f;
            };

            list.onAddCallback = (ReorderableList list) =>
            {
                int index = list.serializedProperty.arraySize;
                list.serializedProperty.arraySize++;
                list.index = index;

                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                element.FindPropertyRelative("name").stringValue = "New Sound";
                element.FindPropertyRelative("volume").floatValue = 1f;
                element.FindPropertyRelative("pitch").floatValue = 1f;
            };

            list.onRemoveCallback = (ReorderableList list) =>
            {
                if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this sound?", "Yes", "No"))
                {
                    ReorderableList.defaultBehaviours.DoRemoveButton(list);
                }
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("persistent"));

            list.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}