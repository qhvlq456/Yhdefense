using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    #region Start Path
    public const string heroLandStr = "HeroLand";
    public const string enemyLandStr = "EnemyLand";
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
    #endregion End Map
    [Header("End Map")]
    [Space()]

    [Header("Start Character")]
    #region Start Character
    private List<HeroData> heroDataList = new List<HeroData>();
    public List<HeroData> HeroDataList => heroDataList;
    public HeroData GetIdxToHeroData(int _idx) => heroDataList.Find(x => x.index == _idx);
    private List<EnemyData> enemyDataList = new List<EnemyData>();
    public EnemyData GetIdxToEnemyData(int _idx) => enemyDataList.Find(x => x.index == _idx);
    
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
    public int GetMaxHeroUpgradeLevel(int _heroIdx)
    {
        if (!heroUpgradeDataDic.ContainsKey(_heroIdx))
        {
            Debug.LogError($"Hero idx {_heroIdx} not found in upgrade data!");
            return 0;
        }
        return heroUpgradeDataDic[_heroIdx].Count;
    }
    #endregion End UpgradeData

    #region Start Hero Attack Data
    [Header("Start Attack Data")]
    // key : hero heroTypeByIdx , value : AttackData List sort by lv
    [SerializeField]
    private Dictionary<int, List<AttackData>> attackDataListDic = new Dictionary<int, List<AttackData>>();
    public AttackData GetAttackData(int _heroTypeByIdx, int _lv)
    {
        if (attackDataListDic.TryGetValue(_heroTypeByIdx, out List<AttackData> attackDataList))
        {
            int safeLevel = Mathf.Clamp(_lv - 1, 0, attackDataList.Count - 1);
            return attackDataList[safeLevel];
        }
        else
        {
            Debug.LogError($"Attack data with heroTypeByIdx {_heroTypeByIdx} not found!");
            return default;
        }
    }
    [Header("End Attack Data")]
    [Space()]
    #endregion End Hero Attack Data

    #region Start Hero Buff Data
    [Header("Start Buff Data")]
    [SerializeField]
    private Dictionary<int, List<BuffData>> buffDataListDic = new Dictionary<int, List<BuffData>>();
    public BuffData GetBuffData(int _heroTypeByIdx, int _lv)
    {
        if (buffDataListDic.TryGetValue(_heroTypeByIdx, out List<BuffData> buffDataList))
        {
            int safeLevel = Mathf.Clamp(_lv - 1, 0, buffDataList.Count - 1);
            return buffDataList[safeLevel];
        }
        else
        {
            Debug.LogError($"Buff data with heroTypeByIdx index {_heroTypeByIdx} not found!");
            return default;
        }
    }
    [Header("End Buff Data")]
    [Space()]
    #endregion End Hero Buff Data
    #region Start Hero Debuff Data
    [Header("Start Debuff Data")]
    [SerializeField]
    private Dictionary<int, List<DebuffData>> debuffDataListDic = new Dictionary<int, List<DebuffData>>();
    public DebuffData GetDebuffData(int _heroTypeByIdx, int _lv)
    {
        if (debuffDataListDic.TryGetValue(_heroTypeByIdx, out List<DebuffData> debuffDataList))
        {
            int safeLevel = Mathf.Clamp(_lv - 1, 0, debuffDataList.Count - 1);
            return debuffDataList[safeLevel];
        }
        else
        {
            Debug.LogError($"Debuff data with heroTypeByIdx {_heroTypeByIdx} not found!");
            return default;
        }
    }
    [Header("End Debuff Data")]
    [Space()]
    #endregion End Hero Debuff Data

    #region Start Move Data
    [Header("Start Move Data")]
    // key : enemy idx , value : MoveData
    [SerializeField]
    private Dictionary<int, MoveData> moveDataDic = new Dictionary<int, MoveData>();
    public MoveData GetMoveData(int _idx)
    {
        if (moveDataDic.TryGetValue(_idx, out MoveData moveData))
        {
            return moveData;
        }
        else
        {
            Debug.LogError($"Move data with index {_idx} not found!");
            return default;
        }
    }
    [Header("End Move Data")]
    [Space()]
    #endregion End Move Data

    [Header("Start Weapon")]
    #region Start Weapon
    private List<WeaponData> weaponDataList = new List<WeaponData>();
    // hero weapon idx 로 검색
    public WeaponData GetWeaponData(int _idx) => weaponDataList.Find(x => x.index == _idx);


    #endregion End Weapon
    public void LoadGameData()
    {
        heroDataList = NewtonSoftJson.LoadJsonArray<HeroData>(Application.streamingAssetsPath, "HeroData");
        enemyDataList = NewtonSoftJson.LoadJsonArray<EnemyData>(Application.streamingAssetsPath, "EnemyData");
        subStageDataList = NewtonSoftJson.LoadJsonArray<SubStageData>(Application.streamingAssetsPath, "SubStageData");
        weaponDataList = NewtonSoftJson.LoadJsonArray<WeaponData>(Application.streamingAssetsPath, "WeaponData");

        Debug.LogError(DataLogger.LogList(heroDataList));

        List<HeroUpgradeData> upgradeDataList = NewtonSoftJson.LoadJsonArray<HeroUpgradeData>(Application.streamingAssetsPath, "HeroUpgradeData");
        heroUpgradeDataDic.Clear();
        foreach (var data in upgradeDataList)
        {
            if (!heroUpgradeDataDic.ContainsKey(data.heroIdx))
            {
                heroUpgradeDataDic[data.heroIdx] = new List<HeroUpgradeData>();
            }

            heroUpgradeDataDic[data.heroIdx].Add(data);
        }
        Debug.LogError(DataLogger.LogDictionary(heroUpgradeDataDic));

        mapDataList.Clear();
        List<StageData> maps = NewtonSoftJson.LoadJsonArray<StageData>(Application.streamingAssetsPath, "MapData");
        for (int i = 0; i < maps.Count; i++)
        {
            MapData data = new MapData();
            data.index = i;
            data.stageData = maps[i];
            mapDataList.Add(data);
        }

        // MoveData
        List<MoveData> moveDataList = NewtonSoftJson.LoadJsonArray<MoveData>(Application.streamingAssetsPath, "MoveData");
        moveDataDic.Clear();
        foreach (var data in moveDataList)
        {
            moveDataDic[data.idx] = data;
        }

        // AttackData (key: herotypebyidx, value: List<AttackData> (lv-1 = index))
        Dictionary<int, List<AttackData>> attackDataListByIdx = new Dictionary<int, List<AttackData>>();
        List<AttackData> attackDataList = NewtonSoftJson.LoadJsonArray<AttackData>(Application.streamingAssetsPath, "AttackData");

        // herotypebyidx별로 그룹화 후, 레벨 순서대로 정렬
        foreach (var data in attackDataList)
        {
            if (!attackDataListByIdx.ContainsKey(data.idx))
            {
                attackDataListByIdx[data.idx] = new List<AttackData>();
            }

            // 리스트의 (lv-1) 위치에 값이 들어가도록 보장
            int insertIdx = data.lv - 1;
            var list = attackDataListByIdx[data.idx];
            // 리스트 크기 보장
            while (list.Count <= insertIdx)
            {
                list.Add(default);
            }
            list[insertIdx] = data;
        }
        attackDataListDic = attackDataListByIdx;

        Debug.LogError(DataLogger.LogDictionary(attackDataListDic));

        // BuffData (idx = HeroData.heroTypebyIdx 기준)
        Dictionary<int, List<BuffData>> buffDataListByIdx = new Dictionary<int, List<BuffData>>();
        List<BuffData> buffDataList = NewtonSoftJson.LoadJsonArray<BuffData>(Application.streamingAssetsPath, "BuffData");
        foreach (var data in buffDataList)
        {
            if (!buffDataListByIdx.ContainsKey(data.idx))
                buffDataListByIdx[data.idx] = new List<BuffData>();

            int insertIdx = data.lv - 1;
            var list = buffDataListByIdx[data.idx];
            // 리스트 크기 보장
            while (list.Count <= insertIdx)
                list.Add(default);

            list[insertIdx] = data;
        }
        buffDataListDic = buffDataListByIdx;
        Debug.LogError(DataLogger.LogDictionary(buffDataListDic));

        // DebuffData (idx = HeroData.heroTypebyIdx 기준)
        Dictionary<int, List<DebuffData>> debuffDataListByIdx = new Dictionary<int, List<DebuffData>>();
        List<DebuffData> debuffDataList = NewtonSoftJson.LoadJsonArray<DebuffData>(Application.streamingAssetsPath, "DebuffData");
        foreach (var data in debuffDataList)
        {
            if (!debuffDataListByIdx.ContainsKey(data.idx))
            {
                debuffDataListByIdx[data.idx] = new List<DebuffData>();
            }

            int insertIdx = data.lv - 1;
            var list = debuffDataListByIdx[data.idx];
            // 리스트 크기 보장
            while (list.Count <= insertIdx)
            {
                list.Add(default);
            }

            list[insertIdx] = data;
        }
        // 필요하다면 멤버 변수로 debuffDataListDic 선언 후 할당
        // debuffDataListDic = debuffDataListByIdx;
    }
    // 일단 보관 나중에 사용할 여지가 있음
    private string GetNameByIndex<T>(List<T> _list, int _idx) where T : IIndexNameData
    {
        var data = _list.Find(x => x.index == _idx);
        return data.name;
    }
    public string GetIdxToObjName(PoolingType _type, int _idx)
    {
        return AddressableManager.Instance.GetAddressableName(_type, _idx);
    }
    public void ResetAllData()
    {

    }
}
