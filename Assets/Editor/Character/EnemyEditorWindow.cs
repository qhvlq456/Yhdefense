#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class EnemyEditorWindow : EditorWindow
{
    private List<EnemyData> enemyList = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Character/Enemy Editor")]
    public static void Open()
    {
        GetWindow<EnemyEditorWindow>("Enemy Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add New Enemy"))
        {
            enemyList.Add(new EnemyData { index = 0, name = "" ,
                maxHealth = 10,
                moveSpeed = 2f, dieGold = 10, groundType = GroundType.gorund });
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < enemyList.Count; i++)
        {
            GUILayout.BeginVertical("box");

            enemyList[i] = new EnemyData
            {
                index = EditorGUILayout.IntField("Index", enemyList[i].index),
                name = EditorGUILayout.TextField("name", enemyList[i].name),
                maxHealth = EditorGUILayout.FloatField("maxHealth", enemyList[i].maxHealth),
                moveSpeed = EditorGUILayout.FloatField("moveSpeed", enemyList[i].moveSpeed),
                dieGold = EditorGUILayout.IntField("Die Gold", enemyList[i].dieGold),
                groundType = (GroundType)EditorGUILayout.EnumPopup("Die Gold", enemyList[i].groundType)
            };

            if (GUILayout.Button("Remove Enemy"))
            {
                enemyList.RemoveAt(i);
                GUILayout.EndVertical();  // 반드시 먼저 EndVertical 호출
                break;
            }

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
        string path = EditorUtility.SaveFilePanel("Save Enemy JSON", Application.dataPath, "EnemyData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = JsonConvert.SerializeObject(enemyList, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("Saved Enemy JSON to " + path);
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load Enemy JSON", Application.dataPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            enemyList = JsonConvert.DeserializeObject<List<EnemyData>>(json);
            Debug.Log("Loaded Enemy JSON from " + path);
        }
    }
}
#endif
