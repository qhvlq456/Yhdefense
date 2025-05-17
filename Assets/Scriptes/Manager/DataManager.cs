using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    #region Start Path
    #endregion End Path

    #region Start Map
    [Header("Start Map")]
    public const int MAX_X = 10;
    public const int MAX_Z = 10;
    public int TotalLandNum => MAX_X * MAX_Z;

    private int xOffset = 1;
    private int zOffset = 1;
    [SerializeField]
    private List<SubStageData> subStageDataList = new List<SubStageData>();
    public SubStageData GetIdxToSubStageData(int _idx) => subStageDataList.Find(x => x.index == _idx);
    private List<MapData> mapDataList = new List<MapData>();
    public StageData GetStageData(int _idx) => mapDataList.Find(x => x.index == _idx).stageData;

    [SerializeField]
    private LandResDB heroLandResDB;
    public GameObject GetHeroLandResObj(int _idx) => heroLandResDB.GetPrefab(_idx);
    [SerializeField]
    private LandResDB enemyLandResDB;
    public GameObject GetEnemyLandResObj(int _idx) => enemyLandResDB.GetPrefab(_idx);
    #endregion End Map
    [Header("End Map")]
    [Space()]

    [Header("Start Character")]
    #region Start Character
    private List<HeroData> heroDataList = new List<HeroData>();
    public HeroData GetIdxToHeroData(int _idx) => heroDataList.Find(x => x.index == _idx);
    private List<EnemyData> enemyDataList = new List<EnemyData>();
    public EnemyData GetIdxToEnemyData(int _idx) => enemyDataList.Find(x => x.index == _idx);
    [SerializeField]
    private CharacterResDB heroResDB;
    public GameObject GetHeroResObj(int _idx) => heroResDB.GetPrefab(_idx);
    [SerializeField]
    private CharacterResDB enemyResDB;
    public GameObject GetEnemyResObj(int _idx) => enemyResDB.GetPrefab(_idx);
    #endregion End Character
    [Header("End Character")]
    [Space()]
    #region Start UpgradeData
    private Dictionary<int, List<HeroUpgradeData>> heroUpgradeDataDic = new Dictionary<int, List<HeroUpgradeData>>();
    public HeroUpgradeData GetHeroUpgradeData(int _heroIdx, int _level)
    {
        if (!heroUpgradeDataDic.ContainsKey(_heroIdx))
        {
            Debug.LogError($"Hero idx {_heroIdx} not found in upgrade data!");
            return default;
        }

        var list = heroUpgradeDataDic[_heroIdx];
        int safeLevel = Mathf.Clamp(_level - 1, 0, list.Count - 1);
        return list[safeLevel];
    }

    #endregion End UpgradeData
    public void LoadGameData()
    {
        heroDataList = NewtonSoftJson.LoadJsonArray<HeroData>(Application.streamingAssetsPath, "HeroData");
        enemyDataList = NewtonSoftJson.LoadJsonArray<EnemyData>(Application.streamingAssetsPath, "EnemyData");
        subStageDataList= NewtonSoftJson.LoadJsonArray<SubStageData>(Application.streamingAssetsPath, "SubStageData");

        List<HeroUpgradeData> upgradeDataList = NewtonSoftJson.LoadJsonArray<HeroUpgradeData>(Application.streamingAssetsPath, "HeroUpgradeData");
        heroUpgradeDataDic.Clear();
        foreach (var data in upgradeDataList)
        {
            if (!heroUpgradeDataDic.ContainsKey(data.heroIdx))
                heroUpgradeDataDic[data.heroIdx] = new List<HeroUpgradeData>();

            heroUpgradeDataDic[data.heroIdx].Add(data);
        }

        mapDataList.Clear();
        List<StageData> maps = NewtonSoftJson.LoadJsonArray<StageData>(Application.streamingAssetsPath, "MapData");
        for (int i = 0; i < maps.Count; i++)
        {
            MapData data = new MapData();
            data.index = i;
            data.stageData = maps[i];
            mapDataList.Add(data);
        }
    }
    public void ResetAllData()
    {

    }
}
