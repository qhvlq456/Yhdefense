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

    [MenuItem("Tools/Character/Hero Editor")]
    public static void Open()
    {
        GetWindow<HeroEditorWindow>("Hero Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add New Hero"))
        {
            heroList.Add(new HeroData
            {
                index = 0,
                weaponIdx = 0,
                name = "NewHero",
                groundType = GroundType.gorund,
                heroType = HeroType.Attack,
                skillIdList = new List<int>()
            });
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < heroList.Count; i++)
        {
            GUILayout.BeginVertical("box");

            heroList[i] = new HeroData
            {
                index = EditorGUILayout.IntField("Index", heroList[i].index),
                weaponIdx = EditorGUILayout.IntField("WeaponIdx", heroList[i].weaponIdx),
                name = EditorGUILayout.TextField("Name", heroList[i].name),
                groundType = (GroundType)EditorGUILayout.EnumPopup("Ground Type", heroList[i].groundType),
                heroType = (HeroType)EditorGUILayout.EnumPopup("Hero Type", heroList[i].heroType),
                skillIdList = DrawSkillList(heroList[i].skillIdList)
            };

            if (GUILayout.Button("Remove Hero"))
            {
                heroList.RemoveAt(i);
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

    private List<int> DrawSkillList(List<int> skillList)
    {
        int count = Mathf.Max(0, EditorGUILayout.IntField("Skill Count", skillList.Count));
        while (skillList.Count < count) skillList.Add(0);
        while (skillList.Count > count) skillList.RemoveAt(skillList.Count - 1);

        for (int j = 0; j < skillList.Count; j++)
        {
            skillList[j] = EditorGUILayout.IntField($"Skill {j}", skillList[j]);
        }

        return skillList;
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