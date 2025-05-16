#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class HeroEditorWindow : EditorWindow
{
    private List<HeroData> heroList = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Hero Editor")]
    public static void Open()
    {
        GetWindow<HeroEditorWindow>("Hero Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add New Hero"))
        {
            heroList.Add(new HeroData { index = 0, cost = 100 });
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < heroList.Count; i++)
        {
            GUILayout.BeginVertical("box");

            heroList[i] = new HeroData
            {
                index = EditorGUILayout.IntField("Index", heroList[i].index),
                cost = EditorGUILayout.IntField("Cost", heroList[i].cost)
            };

            if (GUILayout.Button("Remove Hero"))
                heroList.RemoveAt(i);

            GUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save JSON"))
            SaveJson();

        if (GUILayout.Button("Load JSON"))
            LoadJson();
        EditorGUILayout.EndHorizontal();
    }

    private void SaveJson()
    {
        string path = EditorUtility.SaveFilePanel("Save Hero JSON", Application.dataPath, "HeroData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = JsonConvert.SerializeObject(heroList, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("Saved Hero JSON to " + path);
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load Hero JSON", Application.dataPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            heroList = JsonConvert.DeserializeObject<List<HeroData>>(json);
            Debug.Log("Loaded Hero JSON from " + path);
        }
    }
}
#endif
