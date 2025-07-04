#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class DebuffDataEditorWindow : EditorWindow
{
    // key: idx(herotypebyidx), value: lv 오름차순 DebuffData 리스트
    private Dictionary<int, List<DebuffData>> debuffDataDict = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Attack/Debuff Data Editor")]
    public static void Open()
    {
        GetWindow<DebuffDataEditorWindow>("Debuff Data Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add New Group (idx)"))
        {
            int newIdx = 0;
            while (debuffDataDict.ContainsKey(newIdx)) newIdx++;
            debuffDataDict[newIdx] = new List<DebuffData>();
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        var keys = new List<int>(debuffDataDict.Keys);
        foreach (var idx in keys)
        {
            GUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"[idx: {idx}] DebuffData List", EditorStyles.boldLabel);

            var list = debuffDataDict[idx];

            // 개별 DebuffData 편집
            for (int i = 0; i < list.Count; i++)
            {
                GUILayout.BeginVertical("box");
                list[i] = DrawDebuffData(list[i]);
                if (GUILayout.Button("Remove Level"))
                {
                    list.RemoveAt(i);
                    GUILayout.EndVertical();
                    break;
                }
                GUILayout.EndVertical();
            }

            GUILayout.Space(5);
            if (GUILayout.Button("Add Level"))
            {
                int nextLv = list.Count + 1;
                list.Add(new DebuffData { idx = idx, lv = nextLv });
            }

            GUILayout.Space(5);
            if (GUILayout.Button("Remove This Group"))
            {
                debuffDataDict.Remove(idx);
                GUILayout.EndVertical();
                break;
            }
            GUILayout.EndVertical();
            GUILayout.Space(10);
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

    private DebuffData DrawDebuffData(DebuffData data)
    {
        data.idx = EditorGUILayout.IntField("Idx", data.idx);
        data.lv = EditorGUILayout.IntField("Level", data.lv);
        data.weaponIdx = EditorGUILayout.IntField("WeaponIdx", data.weaponIdx);
        data.debuffType = (BuffType)EditorGUILayout.EnumPopup("DebuffType", data.debuffType);
        data.range = EditorGUILayout.FloatField("Range", data.range);
        data.amount = EditorGUILayout.FloatField("Amount", data.amount);
        data.duration = EditorGUILayout.FloatField("Duration", data.duration);
        data.startTime = EditorGUILayout.FloatField("StartTime", data.startTime);
        return data;
    }

    private void SaveJson()
    {
        string path = EditorUtility.SaveFilePanel("Save DebuffData JSON", Application.streamingAssetsPath, "DebuffData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            // 평탄화해서 저장
            var flatList = debuffDataDict.Values.SelectMany(list => list.OrderBy(d => d.lv)).ToList();
            string json = JsonConvert.SerializeObject(flatList, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("Saved DebuffData JSON to " + path);
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load DebuffData JSON", Application.streamingAssetsPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            var flatList = JsonConvert.DeserializeObject<List<DebuffData>>(json);
            // 그룹화
            debuffDataDict.Clear();
            foreach (var data in flatList)
            {
                if (!debuffDataDict.ContainsKey(data.idx))
                    debuffDataDict[data.idx] = new List<DebuffData>();
                debuffDataDict[data.idx].Add(data);
            }
            // 각 그룹을 lv 오름차순 정렬
            foreach (var key in debuffDataDict.Keys.ToList())
            {
                debuffDataDict[key] = debuffDataDict[key].OrderBy(d => d.lv).ToList();
            }
            Debug.Log("Loaded DebuffData JSON from " + path);
        }
    }
}
#endif