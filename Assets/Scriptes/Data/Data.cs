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
[Serializable]
public struct StageData
{
    public int index;
    public int life;
    public Vector2 startPoint;
    public Vector2 endPoint;

    public List<LandData> landDataList;
    public List<SubStageData> subStageDataList;
}
[Serializable]
public struct SubStageData
{
    public int index;
    public float restTime;
    public List<EnemyData> enemyDataList;
}
[Serializable]
public struct LandData
{
    public int index;
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
[Serializable]
public struct CharacterData
{
    public int index;
    public string resName;
    public string name;
    public CharacterType characterType;
}
[Serializable]
public struct HeroData
{
    public int index;
    public int cost;
}
[Serializable]
public struct EnemyData
{
    public int index;
    public int dieGold;
}

[Serializable]
public struct UIData
{
    public UIType type;
    public UIPanelType panelType;
}

