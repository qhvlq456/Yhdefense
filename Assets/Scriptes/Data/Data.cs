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
    public Vector3 startPoint;
    public Vector3 endPoint;

    public List<LandData> landDataList;
    public List<int> subStageIdxList;
}
[Serializable]
public struct SubStageData
{
    public int index;
    public float restTime;
    // 후에 int 즉 enemy index로 수정
    public List<int> enemyIdxList;
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
public struct HeroData
{
    public int index;
    public string name;
    public GroundType groundType;
    public HeroType heroType;

    public List<int> skillIdList; // 스킬 시스템 추가 대비
}
[Serializable]
public struct EnemyData
{
    public int index;
    public string name;
    public float maxHealth;
    public float moveSpeed;
    public int dieGold;
    public GroundType groundType;
}

[Serializable]
public struct SkillData
{
    public int index;
    public string name;
    public float cooldown;
    public float range;
    public float power;
    public SkillType skillType;
}

[Serializable]
public struct HeroUpgradeData
{
    public int heroIdx; // == hero idx 
    public int cost;
    public float attackSpeed;
    public float attackDamage;
    public float attackRadius;

    public float buffValue; // buffer/debuffer에 유용
}


