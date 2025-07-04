#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class BuffDataEditorWindow : EditorWindow
{
    // key: idx(herotypebyidx), value: lv 오름차순 BuffData 리스트
    private Dictionary<int, List<BuffData>> buffDataDict = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Attack/Buff Data Editor")]
    public static void Open()
    {
        GetWindow<BuffDataEditorWindow>("Buff Data Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add New Group (idx)"))
        {
            int newIdx = 0;
            while (buffDataDict.ContainsKey(newIdx)) newIdx++;
            buffDataDict[newIdx] = new List<BuffData>();
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        var keys = new List<int>(buffDataDict.Keys);
        foreach (var idx in keys)
        {
            GUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"[idx: {idx}] BuffData List", EditorStyles.boldLabel);

            var list = buffDataDict[idx];

            // 개별 BuffData 편집
            for (int i = 0; i < list.Count; i++)
            {
                GUILayout.BeginVertical("box");
                BuffData data = list[i];
                data = DrawBuffData(data); // 수정
                list[i] = data;            // 다시 할당!
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
                list.Add(new BuffData { idx = idx, lv = nextLv });
            }

            GUILayout.Space(5);
            if (GUILayout.Button("Remove This Group"))
            {
                buffDataDict.Remove(idx);
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

    private BuffData DrawBuffData(BuffData data)
    {
        data.idx = EditorGUILayout.IntField("Idx", data.idx);
        data.lv = EditorGUILayout.IntField("Level", data.lv);
        data.weaponIdx = EditorGUILayout.IntField("WeaponIdx", data.weaponIdx);
        data.buffType = (BuffType)EditorGUILayout.EnumPopup("BuffType", data.buffType);
        data.range = EditorGUILayout.FloatField("Range", data.range);
        data.amount = EditorGUILayout.FloatField("Amount", data.amount);
        data.duration = EditorGUILayout.FloatField("Duration", data.duration);
        data.interval = EditorGUILayout.FloatField("Interval", data.interval);
        // startTime, nextApplyTime은 런타임에서만 사용하므로 0으로 고정
        data.startTime = 0f;
        data.nextApplyTime = 0f;
        return data;
    }

    private void SaveJson()
    {
        string path = EditorUtility.SaveFilePanel("Save BuffData JSON", Application.streamingAssetsPath, "BuffData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            // Dictionary를 평탄화(flatten)해서 List<BuffData>로 만듦
            List<BuffData> flatList = buffDataDict.Values.SelectMany(list => list.OrderBy(d => d.lv)).ToList();
            for (int i = 0; i < flatList.Count; i++)
            {
                BuffData buffData = flatList[i];
                buffData.startTime = 0f;
                buffData.nextApplyTime = 0f;
                flatList[i] = buffData; // startTime, nextApplyTime은 런타임에서만 사용하므로 0으로 고정

            }
            string json = JsonConvert.SerializeObject(flatList, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("Saved BuffData JSON to " + path);
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load BuffData JSON", Application.streamingAssetsPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            var flatList = JsonConvert.DeserializeObject<List<BuffData>>(json);
            // 그룹화
            buffDataDict.Clear();
            foreach (var data in flatList)
            {
                if (!buffDataDict.ContainsKey(data.idx))
                    buffDataDict[data.idx] = new List<BuffData>();
                buffDataDict[data.idx].Add(data);
            }
            // 각 그룹을 lv 오름차순 정렬
            foreach (var key in buffDataDict.Keys.ToList())
            {
                buffDataDict[key] = buffDataDict[key].OrderBy(d => d.lv).ToList();
            }
            Debug.Log("Loaded BuffData JSON from " + path);
        }
    }
}
#endif