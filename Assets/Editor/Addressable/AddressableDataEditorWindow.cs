#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class AddressableDataEditorWindow : EditorWindow
{
    private List<AddressableData> addressableList = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Addressables/Addressable Data Editor")]
    public static void Open()
    {
        GetWindow<AddressableDataEditorWindow>("Addressable Data Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add New AddressableData"))
        {
            addressableList.Add(new AddressableData
            {
                type = PoolingType.hero,
                key = "",
                idx = 0,
                groupName = "DefaultGroup",
                label = "DefaultLabel"
            });
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < addressableList.Count; i++)
        {
            GUILayout.BeginVertical("box");

            addressableList[i] = new AddressableData
            {
                type = (PoolingType)EditorGUILayout.EnumPopup("Type", addressableList[i].type),
                key = EditorGUILayout.TextField("Name", addressableList[i].key),
                idx = EditorGUILayout.IntField("Index", addressableList[i].idx),
                groupName = EditorGUILayout.TextField("GroupName", addressableList[i].groupName),
                label = EditorGUILayout.TextField("LabelName", addressableList[i].label)
            };

            if (GUILayout.Button("Remove"))
            {
                addressableList.RemoveAt(i);
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

    private void SaveJson()
    {
        string path = EditorUtility.SaveFilePanel("Save AddressableData JSON", Application.streamingAssetsPath, "AddressableData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = JsonConvert.SerializeObject(addressableList, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("Saved AddressableData JSON to " + path);
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load AddressableData JSON", Application.dataPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            addressableList = JsonConvert.DeserializeObject<List<AddressableData>>(json);
            Debug.Log("Loaded AddressableData JSON from " + path);
        }
    }
}
#endif
