#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class AttackDataEditorWindow : EditorWindow
{
    // key: idx(herotypebyidx), value: lv 오름차순 AttackData 리스트
    private Dictionary<int, List<AttackData>> attackDataDict = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Attack/AttackData Editor")]
    public static void Open()
    {
        GetWindow<AttackDataEditorWindow>("AttackData Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add New Group (idx)"))
        {
            int newIdx = 0;
            while (attackDataDict.ContainsKey(newIdx)) newIdx++;
            attackDataDict[newIdx] = new List<AttackData>();
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        var keys = new List<int>(attackDataDict.Keys);
        foreach (var idx in keys)
        {
            GUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"[idx: {idx}] AttackData List", EditorStyles.boldLabel);

            var list = attackDataDict[idx];

            // 개별 AttackData 편집
            for (int i = 0; i < list.Count; i++)
            {
                GUILayout.BeginVertical("box");
                list[i] = DrawAttackData(list[i]);
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
                list.Add(new AttackData { idx = idx, lv = nextLv });
            }

            GUILayout.Space(5);
            if (GUILayout.Button("Remove This Group"))
            {
                attackDataDict.Remove(idx);
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

    private AttackData DrawAttackData(AttackData data)
    {
        data.idx = EditorGUILayout.IntField("Idx", data.idx);
        data.lv = EditorGUILayout.IntField("Level", data.lv);
        data.weaponIdx = EditorGUILayout.IntField("WeaponIdx", data.weaponIdx);
        data.targetCount = EditorGUILayout.IntField("TargetCount", data.targetCount);
        data.speed = EditorGUILayout.FloatField("Speed", data.speed);
        data.damage = EditorGUILayout.FloatField("Damage", data.damage);
        data.radius = EditorGUILayout.FloatField("Radius", data.radius);
        return data;
    }

    private void SaveJson()
    {
        string path = EditorUtility.SaveFilePanel("Save AttackData JSON", Application.dataPath, "AttackData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            // 평탄화해서 저장
            var flatList = attackDataDict.Values.SelectMany(list => list.OrderBy(d => d.lv)).ToList();
            string json = JsonConvert.SerializeObject(flatList, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("Saved AttackData JSON to " + path);
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load AttackData JSON", Application.dataPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            var flatList = JsonConvert.DeserializeObject<List<AttackData>>(json);
            // 그룹화
            attackDataDict.Clear();
            foreach (var data in flatList)
            {
                if (!attackDataDict.ContainsKey(data.idx))
                    attackDataDict[data.idx] = new List<AttackData>();
                attackDataDict[data.idx].Add(data);
            }
            // 각 그룹을 lv 오름차순 정렬
            foreach (var key in attackDataDict.Keys.ToList())
            {
                attackDataDict[key] = attackDataDict[key].OrderBy(d => d.lv).ToList();
            }
            Debug.Log("Loaded AttackData JSON from " + path);
        }
    }
}
#endif