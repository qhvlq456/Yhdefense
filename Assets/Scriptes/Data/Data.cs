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
    // �Ŀ� int �� enemy index�� ����
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
// dictionary �� �ϸ� �ν����Ϳ� �Ⱥ����� ����ü�� ��ȯ
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

    public List<int> skillIdList; // ��ų �ý��� �߰� ���
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

    public float buffValue; // buffer/debuffer�� ����
}


