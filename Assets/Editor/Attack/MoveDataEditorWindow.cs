#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class MoveDataEditorWindow : EditorWindow
{
    private List<MoveData> moveDataList = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Attack/Move Data Editor")]
    public static void Open()
    {
        GetWindow<MoveDataEditorWindow>("Move Data Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add New MoveData"))
        {
            moveDataList.Add(new MoveData());
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < moveDataList.Count; i++)
        {
            GUILayout.BeginVertical("box");

            moveDataList[i] = DrawMoveData(moveDataList[i]);

            if (GUILayout.Button("Remove"))
            {
                moveDataList.RemoveAt(i);
                GUILayout.EndVertical();
                break;
            }

            GUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save JSON"))
        {
            SaveJson();
        }

        if (GUILayout.Button("Load JSON"))
        {
            LoadJson();
        }

        EditorGUILayout.EndHorizontal();
    }

    private MoveData DrawMoveData(MoveData data)
    {
        data.idx = EditorGUILayout.IntField("Idx", data.idx);
        data.moveSpeed = EditorGUILayout.FloatField("Move Speed", data.moveSpeed);
        data.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", data.rotationSpeed);
        data.stoppingDistance = EditorGUILayout.FloatField("Stopping Distance", data.stoppingDistance);
        return data;
    }

    private void SaveJson()
    {
        string path = EditorUtility.SaveFilePanel("Save MoveData JSON", Application.streamingAssetsPath, "MoveData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = JsonConvert.SerializeObject(moveDataList, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("Saved MoveData JSON to " + path);
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load MoveData JSON", Application.streamingAssetsPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            moveDataList = JsonConvert.DeserializeObject<List<MoveData>>(json);
            Debug.Log("Loaded MoveData JSON from " + path);
        }
    }
}
#endif