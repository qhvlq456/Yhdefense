using UnityEngine;
using System;
using System.Collections.Generic;

public struct PlayerData
{
    public int guid;
    public int gold;
    public int ruby;
    public int stage;
    public string name;
}
public struct StageData
{
    public int stage;
    public int life;
    public List<LandData> landDataList;
    public List<SubStageData> subStageDataList;
}
public struct SubStageData
{
    public int index;
    public float restTime;
    public List<EnemyData> enemyDataList;
}
public struct LandData
{
    // col
    public int x;
    // row
    public int z;
    public LandType landType;
}
// dictionary 로 하면 인스펙터에 안보여서 구조체로 변환
[Serializable]
public struct MapData
{
    public int index;
    public StageData stageData;
}

public struct CharacterData
{
    public int index;
    public CharacterType characterType;
}
public struct HeroData
{
    public CharacterData characterData;
}
public struct EnemyData
{
    public CharacterData characterData;
    public int dieGold;
}


