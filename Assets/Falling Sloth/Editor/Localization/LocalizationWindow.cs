using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FallingSloth.Localization
{
    public class LocalizationWindow : EditorWindow
    {
        string filename = "Assets/Falling Sloth/Scripts/Localization/KeywordList.cs";
        List<string> keywords;

        [MenuItem("Tools/Localization")]
        static void Init()
        {
            EditorWindow.GetWindow<LocalizationWindow>().Show();
        }

        void OnEnable()
        {
            if (File.Exists(filename))
                Debug.Log("File found!");
            else
                Debug.Log("File not found!");

            keywords = new List<string>();

            using (StreamReader sr = new StreamReader(filename))
            {
                // read past enum declaration
                sr.ReadLine();

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line == "}}")
                        break;

                    line = line.Trim(' ', ',');
                    keywords.Add(line);
                }
            }
        }

        void SaveFile()
        {
            using (StreamWriter sw = new StreamWriter(filename))
            {
                sw.WriteLine("namespace FallingSloth.Localization{public enum KeywordList{");

                for (int i = 0; i < keywords.Count; i++)
                {
                    if (i == keywords.Count - 1)
                        sw.WriteLine(keywords[i]);
                    else
                        sw.WriteLine(keywords[i] + ",");
                }

                sw.Write("}}");
            }
        }

        void OnGUI()
        {
            StringBuilder values = new StringBuilder();
            foreach (string s in keywords) values.AppendLine(s);

            GUILayout.TextArea(values.ToString());
        }
    }
}