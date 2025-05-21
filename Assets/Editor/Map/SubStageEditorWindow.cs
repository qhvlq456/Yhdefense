#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class SubStageEditorWindow : EditorWindow
{
    private List<SubStageData> subStageList = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Map/SubStage Editor")]
    public static void Open()
    {
        GetWindow<SubStageEditorWindow>("SubStage Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add New SubStage"))
        {
            subStageList.Add(new SubStageData { index = 0, restTime = 5f, enemyIdxList = new List<int>() });
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < subStageList.Count; i++)
        {
            GUILayout.BeginVertical("box");
            var subStage = subStageList[i];

            subStage.index = EditorGUILayout.IntField("Index", subStage.index);
            subStage.restTime = EditorGUILayout.FloatField("Rest Time", subStage.restTime);

            EditorGUILayout.LabelField("Enemy Index List");
            if (GUILayout.Button("Add Enemy Index"))
                subStage.enemyIdxList.Add(0);

            for (int j = 0; j < subStage.enemyIdxList.Count; j++)
            {
                EditorGUILayout.BeginHorizontal();
                subStage.enemyIdxList[j] = EditorGUILayout.IntField($"Enemy {j}", subStage.enemyIdxList[j]);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    subStage.enemyIdxList.RemoveAt(j);
                    EditorGUILayout.EndHorizontal();
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Remove SubStage"))
            {
                GUILayout.EndVertical(); //  반드시 EndVertical 호출 후 제거
                subStageList.RemoveAt(i);
                i--;
                continue;
            }

            subStageList[i] = subStage;
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
        string path = EditorUtility.SaveFilePanel("Save SubStage JSON", Application.dataPath, "SubStageData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = JsonConvert.SerializeObject(subStageList, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("Saved SubStage JSON to " + path);
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load SubStage JSON", Application.dataPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            subStageList = JsonConvert.DeserializeObject<List<SubStageData>>(json);
            Debug.Log("Loaded SubStage JSON from " + path);
        }
    }
}
#endif
