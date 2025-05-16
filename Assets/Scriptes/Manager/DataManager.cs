using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    #region Start Path
    #endregion End Path

    #region Start Map
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

    #region Start UIData
    [SerializeField]
    private UIResDB uiMainResDB;
    [SerializeField]
    private UIResDB uiContextResDB;
    [SerializeField]
    private UIResDB uiTooltipResDB;
    [SerializeField]
    private UIResDB uiPopupResDB;

    public GameObject GetUIResObj(UIData _uiData)
    {
        UIResDB uiResDB = null;

        switch (_uiData.type)
        {
            case UIType.main:
                uiResDB = uiMainResDB;
                break;
            case UIType.context:
                uiResDB = uiContextResDB;
                break;
            case UIType.tooltip:
                uiResDB = uiTooltipResDB;
                break;
            case UIType.popup:
                uiResDB = uiPopupResDB;
                break;
        }

        return uiResDB.GetPrefab(_uiData.panelType);
    }
    #endregion End UIDAta
    public void LoadGameData()
    {
        heroDataList = NewtonSoftJson.LoadJsonArray<HeroData>(Application.streamingAssetsPath, "HeroData");
        enemyDataList = NewtonSoftJson.LoadJsonArray<EnemyData>(Application.streamingAssetsPath, "EnemyData");

        subStageDataList= NewtonSoftJson.LoadJsonArray<SubStageData>(Application.streamingAssetsPath, "SubStageData");
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
