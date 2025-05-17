#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class HeroUpgradeEditorWindow : EditorWindow
{
    private Dictionary<int, List<HeroUpgradeData>> upgradeDataDic = new();
    private Vector2 scrollPos;

    private int currentHeroIdx = 0;
    private List<HeroUpgradeData> currentHeroUpgrades =>
        upgradeDataDic.ContainsKey(currentHeroIdx) ? upgradeDataDic[currentHeroIdx] : new List<HeroUpgradeData>();

    [MenuItem("Tools/Hero Upgrade Editor")]
    public static void Open()
    {
        GetWindow<HeroUpgradeEditorWindow>("Hero Upgrade Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Hero Upgrade Editor", EditorStyles.boldLabel);

        currentHeroIdx = EditorGUILayout.IntField("Hero Index", currentHeroIdx);

        if (!upgradeDataDic.ContainsKey(currentHeroIdx))
            upgradeDataDic[currentHeroIdx] = new List<HeroUpgradeData>();

        if (GUILayout.Button("Add Upgrade Level"))
        {
            upgradeDataDic[currentHeroIdx].Add(new HeroUpgradeData
            {
                heroIdx = currentHeroIdx,
                cost = 100,
                attackSpeed = 1f,
                attackDamage = 10f,
                attackRadius = 3f,
                buffValue = 0f
            });
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        List<HeroUpgradeData> list = upgradeDataDic[currentHeroIdx];
        for (int i = 0; i < list.Count; i++)
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label($"Level {i + 1}", EditorStyles.boldLabel);

            var data = list[i];
            data.cost = EditorGUILayout.IntField("Cost", data.cost);
            data.attackSpeed = EditorGUILayout.FloatField("Attack Speed", data.attackSpeed);
            data.attackDamage = EditorGUILayout.FloatField("Attack Damage", data.attackDamage);
            data.attackRadius = EditorGUILayout.FloatField("Attack Radius", data.attackRadius);
            data.buffValue = EditorGUILayout.FloatField("Buff Value", data.buffValue);

            list[i] = data;

            if (GUILayout.Button("Remove Level"))
            {
                list.RemoveAt(i);
                break;
            }

            GUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save JSON")) SaveJson();
        if (GUILayout.Button("Load JSON")) LoadJson();
        EditorGUILayout.EndHorizontal();
    }

    private void SaveJson()
    {
        string path = EditorUtility.SaveFilePanel("Save HeroUpgradeData JSON", Application.dataPath, "HeroUpgradeData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            var flatList = new List<HeroUpgradeData>();
            foreach (var kv in upgradeDataDic)
                flatList.AddRange(kv.Value);

            string json = JsonConvert.SerializeObject(flatList, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("Saved HeroUpgradeData JSON to " + path);
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load HeroUpgradeData JSON", Application.dataPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            var flatList = JsonConvert.DeserializeObject<List<HeroUpgradeData>>(json);
            upgradeDataDic.Clear();
            foreach (var data in flatList)
            {
                if (!upgradeDataDic.ContainsKey(data.heroIdx))
                    upgradeDataDic[data.heroIdx] = new List<HeroUpgradeData>();

                upgradeDataDic[data.heroIdx].Add(data);
            }

            Debug.Log("Loaded HeroUpgradeData JSON from " + path);
        }
    }
}
#endif
