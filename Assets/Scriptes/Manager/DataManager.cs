using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    #region Path
    #endregion End Path
    #region Map
    public const int MAX_X = 10;
    public const int MAX_Z = 10;
    public int TotalLandNum => MAX_X * MAX_Z;

    private int xOffset = 1;
    private int zOffset = 1;

    public List<MapData> mapDataList = new List<MapData>();
    public StageData GetStageData(int _idx) => mapDataList.Find(x => x.index == _idx).stageData;
    [SerializeField]
    private LandResDB heroLandResDB;
    public GameObject GetHeroLandResObj(int _idx) => heroLandResDB.GetPrefab(_idx);
    [SerializeField]
    private LandResDB enemyLandResDB;
    public GameObject GetEnemyLandResObj(int _idx) => enemyLandResDB.GetPrefab(_idx);
    #endregion End Map

    #region Character
    private List<HeroData> heroDataList = new List<HeroData>();
    private List<EnemyData> enemyDataList = new List<EnemyData>();

    [SerializeField]
    private CharacterResDB heroResDB;
    public GameObject GetHeroResObj(int _idx) => heroResDB.GetPrefab(_idx);
    [SerializeField]
    private CharacterResDB enemyResDB;
    public GameObject GetEnemyResObj(int _idx) => enemyResDB.GetPrefab(_idx);
    #endregion End Character

    public void LoadGameData()
    {
        heroDataList = NewtonSoftJson.LoadJsonArray<HeroData>(Application.streamingAssetsPath, "HeroData");
        enemyDataList = NewtonSoftJson.LoadJsonArray<EnemyData>(Application.streamingAssetsPath, "EnemyData");

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
