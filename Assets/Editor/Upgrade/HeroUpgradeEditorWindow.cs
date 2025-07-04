#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class HeroUpgradeEditorWindow : EditorWindow
{
    // 기존 영웅별 업그레이드 데이터 딕셔너리
    private Dictionary<int, List<HeroUpgradeData>> upgradeDataDic = new();

    // 전체 영웅 업그레이드 데이터 리스트 (여러 영웅 데이터 묶음 저장용)
    private List<List<HeroUpgradeData>> heroUpgradeDataList = new();

    private Vector2 scrollPos;
    private int currentHeroIdx = 0;

    private List<HeroUpgradeData> currentHeroUpgrades =>
        upgradeDataDic.ContainsKey(currentHeroIdx) ? upgradeDataDic[currentHeroIdx] : new List<HeroUpgradeData>();

    [MenuItem("Tools/Upgrade/Hero Upgrade Editor")]
    public static void Open()
    {
        GetWindow<HeroUpgradeEditorWindow>("Hero Upgrade Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Hero Upgrade Editor", EditorStyles.boldLabel);

        int newHeroIdx = EditorGUILayout.IntField("Hero Index", currentHeroIdx);
        if (newHeroIdx != currentHeroIdx)
        {
            SaveCurrentHeroData();
            currentHeroIdx = newHeroIdx;
            LoadHeroData(currentHeroIdx);
        }

        if (!upgradeDataDic.ContainsKey(currentHeroIdx))
            upgradeDataDic[currentHeroIdx] = new List<HeroUpgradeData>();

        if (GUILayout.Button("Add Upgrade Level"))
        {
            upgradeDataDic[currentHeroIdx].Add(new HeroUpgradeData
            {
                heroIdx = currentHeroIdx,
                cost = 100,
                sell = 50,
            });
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        List<HeroUpgradeData> list = upgradeDataDic[currentHeroIdx];
        for (int i = 0; i < list.Count; i++)
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label($"Level {i + 1}", EditorStyles.boldLabel);

            var data = list[i];
            data.heroIdx = EditorGUILayout.IntField("hero Index", data.heroIdx);
            data.lv = i + 1; // 레벨은 리스트 인덱스 + 1로 설정
            data.cost = EditorGUILayout.IntField("Cost", data.cost);
            data.sell = EditorGUILayout.IntField("Sell", data.sell);

            list[i] = data;

            if (GUILayout.Button("Remove Level"))
            {
                list.RemoveAt(i);
                GUILayout.EndVertical();  // 반드시 먼저 EndVertical 호출
                break;
            }

            GUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Current Hero To List"))
        {
            AddCurrentHeroToList();
        }

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

    private void AddCurrentHeroToList()
    {
        if (!upgradeDataDic.ContainsKey(currentHeroIdx))
        {
            Debug.LogWarning("현재 영웅 인덱스에 데이터가 없습니다.");
            return;
        }

        List<HeroUpgradeData> currentListCopy = new List<HeroUpgradeData>(upgradeDataDic[currentHeroIdx]);

        if (currentListCopy.Count == 0)
        {
            Debug.LogWarning("추가할 업그레이드 레벨 데이터가 없습니다.");
            return;
        }

        heroUpgradeDataList.Add(currentListCopy);

        Debug.Log($"Hero {currentHeroIdx}의 업그레이드 레벨 데이터가 리스트에 추가되었습니다. 총 리스트 수: {heroUpgradeDataList.Count}");

        // 인덱스 증가 및 초기화
        currentHeroIdx++;
        if (!upgradeDataDic.ContainsKey(currentHeroIdx))
            upgradeDataDic[currentHeroIdx] = new List<HeroUpgradeData>();
    }

    private void SaveJson()
    {
        string path = EditorUtility.SaveFilePanel("Save HeroUpgradeData JSON", Application.dataPath, "HeroUpgradeData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            // 저장할 데이터는 heroUpgradeDataList를 평탄화(flatten)해서 저장
            var flatList = new List<HeroUpgradeData>();
            foreach (var heroList in heroUpgradeDataList)
                flatList.AddRange(heroList);

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

            heroUpgradeDataList.Clear();
            upgradeDataDic.Clear();

            // 불러온 데이터를 영웅별 딕셔너리로 분류하고, 전체 리스트도 재구성
            foreach (var data in flatList)
            {
                if (!upgradeDataDic.ContainsKey(data.heroIdx))
                    upgradeDataDic[data.heroIdx] = new List<HeroUpgradeData>();
                upgradeDataDic[data.heroIdx].Add(data);
            }

            foreach (var kv in upgradeDataDic)
            {
                heroUpgradeDataList.Add(new List<HeroUpgradeData>(kv.Value));
            }

            if (!upgradeDataDic.ContainsKey(currentHeroIdx))
                currentHeroIdx = upgradeDataDic.Count > 0 ? new List<int>(upgradeDataDic.Keys)[0] : 0;

            Debug.Log("Loaded HeroUpgradeData JSON from " + path);
        }
    }

    private void SaveCurrentHeroData()
    {
        if (!upgradeDataDic.ContainsKey(currentHeroIdx)) return;

        List<HeroUpgradeData> currentList = upgradeDataDic[currentHeroIdx];
        for (int i = 0; i < currentList.Count; i++)
        {
            HeroUpgradeData data = currentList[i];
            data.heroIdx = currentHeroIdx;
            currentList[i] = data;
        }
    }

    private void LoadHeroData(int _heroIdx)
    {
        if (!upgradeDataDic.ContainsKey(_heroIdx))
        {
            upgradeDataDic[_heroIdx] = new List<HeroUpgradeData>();
        }
    }
}
#endif
