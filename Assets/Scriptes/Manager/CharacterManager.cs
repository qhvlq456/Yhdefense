using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterManager : Singleton<CharacterManager>, ILoader
{
    private List<HeroData> heroDataList = new List<HeroData>();
    private List<EnemyData> enemyDataList = new List<EnemyData>();

    [SerializeField]
    private CharacterResDB heroResDB;
    public GameObject GetHeroResObj(int _idx) => heroResDB.GetPrefab(_idx);
    [SerializeField]
    private CharacterResDB enemyResDB;
    public GameObject GetEnemyResObj(int _idx) => enemyResDB.GetPrefab(_idx);
    public int Priority { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Load(string _path)
    {
        heroDataList = NewtonSoftJson.LoadJsonArray<HeroData>(Application.streamingAssetsPath, "HeroData");
        enemyDataList = NewtonSoftJson.LoadJsonArray<EnemyData>(Application.streamingAssetsPath, "enemyData");
    }

    // spawn 및 건설 현장 
}
