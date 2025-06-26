using UnityEngine;
using System;
using System.Collections.Generic;

public interface IIndexNameData
{
    int index { get; }
    string name { get; }
}
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
public struct LandData : IIndexNameData
{
    public int index;
    public string name;
    // col
    public int x;
    // row
    public int z;
    public LandType landType;

    int IIndexNameData.index => index;

    string IIndexNameData.name => name;
}
// dictionary 로 하면 인스펙터에 안보여서 구조체로 변환
[Serializable]
public struct MapData
{
    public int index;
    public StageData stageData;
}
[Serializable]
public struct HeroData : IIndexNameData
{
    public int index;
    public string name;
    public GroundType groundType;
    public HeroType heroType;

    public List<int> skillIdList; // 스킬 시스템 추가 대비

    int IIndexNameData.index => index;

    string IIndexNameData.name => name;
}
[Serializable]
public struct EnemyData : IIndexNameData
{
    public int index;
    public string name;
    public float maxHealth;
    // Start MoveData
    public float moveSpeed;
    public float rotationSpeed;
    public float stoppingDistance;
    // End MoveData
    public int dieGold;
    public GroundType groundType;

    int IIndexNameData.index => index;

    string IIndexNameData.name => name;
}

[Serializable]
public struct SkillData : IIndexNameData
{
    public int index;
    public string name;
    public float cooldown;
    public float range;
    public float power;
    public SkillType skillType;

    int IIndexNameData.index => index;

    string IIndexNameData.name => name;
}

// 기본 hero의 attack, buff, debuff data 정보
[Serializable]
public struct HeroUpgradeData
{
    public int heroIdx; // == hero idx 
    public int weaponIdx; // == weapon idx ;;; attack : weapon index, buff : buff idx, debuff : debuffidx 
    public int cost;
    public int sell;
    // attack data 에 필요?
    public int targetCount;
    public float attackSpeed;
    public float attackDamage;
    public float attackRadius;
    // end

    // 아마 여기까지?
    // buffer/debuffer에 필요한 데이터
    public float buffValue; // buffer/debuffer에 유용
}

[Serializable]
public struct WeaponData : IIndexNameData
{
    // weapon idx
    public int index;
    public float speed;
    public string name;
    public WeapondType weaponType;

    int IIndexNameData.index => index;

    string IIndexNameData.name => name;
}

[Serializable]
public struct MoveData
{
    public float moveSpeed;
    public float rotationSpeed;
    public float stoppingDistance; // NavMeshMove에서 사용

    public MoveData(EnemyData _enemyData)
    {
        moveSpeed = _enemyData.moveSpeed;
        rotationSpeed = _enemyData.rotationSpeed;
        stoppingDistance = _enemyData.stoppingDistance;
    }
}
[Serializable]
public struct AttackData
{
    public AttackData(AttackData _heroData)
    {

    }
}
[Serializable]
public struct BuffData
{
    public BuffType buffType;
    public float range;
    public float amount;
    public float duration;
    public float startTime; // 적용 시각
}
[Serializable]
public struct DebuffData
{
    public BuffType debuffType;
    public float range;
    public float amount;
    public float duration;
    public float startTime;
}

[Serializable]
public struct AddressableData
{
    public PoolingType type;       // ex) Hero, Enemy, etc.
    public int idx;                // 고유 인덱스 (데이터 테이블 기준)
    public string key;             // Addressables 키값
    public string groupName;       // 선택 사항: 그룹 정보
    public string label;           // 선택 사항: 특정 label 구분
}
