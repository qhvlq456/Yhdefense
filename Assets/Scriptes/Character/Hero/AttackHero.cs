using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class AttackHero : Hero, IBuffable
{
    [SerializeField]
    protected Attack attack;

    private List<BuffData> activeBuffs = new();
    [SerializeField]
    private float currentAttackDamage;
    [SerializeField]
    private float currentAttackSpeed;
    [SerializeField]
    private float currentAttackRange;

    public override void Set(int _idx)
    {
        base.Set(_idx);
        AttackData attackData = DataManager.Instance.GetAttackData(heroData.heroTypeByIdx, lv);
        attack.Set(attackData, heroData.groundType);
        attack.ShowRange(true);
        Debug.LogError($"[Attack Hero][Set] , upgrade complite after lv : {lv}");
        InitStats();
    }

    protected override void OnAfterUpgrade()
    {
        AttackData attackData = DataManager.Instance.GetAttackData(heroData.heroTypeByIdx, lv);
        attack.Set(attackData, heroData.groundType);
        Debug.LogError($"[Attack Hero][OnAfterUpgrade] , upgrade complite after lv : {lv}");
        RecalculateStats();
    }

    private void Update()
    {
        UpdateBuffs();

        if (attack != null)
        {
            attack.OnUpdateAttack();
            // 후에 볼 필요 있음!
            if (attack.IsAttack())
            {
                attack.Execute(currentAttackDamage, currentAttackSpeed);
            }
        }
    }

    private void InitStats()
    {
        currentAttackDamage = attack.AttackData.damage;
        currentAttackRange = attack.AttackData.radius; // 공격 범위 초기화 (필요시 추가)
        currentAttackSpeed = attack.AttackData.speed;
    }

    private void UpdateBuffs()
    {
        float now = Time.time;
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            var buff = activeBuffs[i];
            // 버프가 활성 상태
            if (buff.duration > 0 && now - buff.startTime < buff.duration)
            {
                // 아직 지속 중
                continue;
            }
            // 버프가 만료됨, interval(쿨타임) 체크
            if (buff.interval > 0)
            {
                if (now >= buff.startTime + buff.duration + buff.interval)
                {
                    // 버프 재적용
                    buff.startTime = now;
                    activeBuffs[i] = buff;
                    RecalculateStats();
                }
                // 아직 대기 중이면 아무것도 안 함
            }
            else
            {
                // 반복 버프가 아니면 제거
                activeBuffs.RemoveAt(i);
                RecalculateStats();
            }
        }
    }
    public override string GetHeroInfo()
    {
        System.Text.StringBuilder sb = new();
        sb.AppendLine("캐릭터:");

        float baseDamage = attack.AttackData.damage;
        float baseSpeed = attack.AttackData.speed;
        float baseRange = attack.AttackData.radius;

        float bonusDamage = 0f;
        float bonusSpeed = 0f;
        float bonusRange = 0f;

        foreach (var buff in activeBuffs)
        {
            switch (buff.buffType)
            {
                case BuffType.attackUp:
                    bonusDamage += buff.amount;
                    break;
                case BuffType.attackSpeed:
                    bonusSpeed += buff.amount;
                    break;
                case BuffType.addRange:
                    bonusRange += buff.amount;
                    break;
            }
        }

        sb.AppendLine($"- 공격력: {baseDamage} (+{bonusDamage}) 총 공격력 : {currentAttackDamage}");
        sb.AppendLine($"- 공격속도: {baseSpeed} (+{bonusSpeed}) 총 공격속도 : {currentAttackSpeed}");
        sb.AppendLine($"- 공격범위: {baseRange} (+{bonusRange}) 총 공격범위 : {currentAttackRange}");

        return sb.ToString();
    }
    public void ApplyBuff(BuffData _buff)
    {
        _buff.startTime = Time.time;
        activeBuffs.Add(_buff);
        RecalculateStats();
    }

    public void RemoveBuff(BuffType _buffType)
    {
        activeBuffs.RemoveAll(b => b.buffType == _buffType);
        RecalculateStats();
    }

    private void RecalculateStats()
    {
        InitStats();
        foreach (var buff in activeBuffs)
        {
            switch (buff.buffType)
            {
                case BuffType.attackUp:
                    currentAttackDamage += buff.amount;
                    break;
                case BuffType.attackSpeed:
                    currentAttackSpeed += buff.amount;
                    break;
                case BuffType.addRange:
                    // 공격 범위 증가 로직 필요
                    currentAttackRange += buff.amount;
                    // 예: attack.IncreaseRange(buff.amount);
                    break;
            }
        }
    }

    public bool HasBuff(BuffType _buffType)
    {
        return activeBuffs.Exists(b => b.buffType == _buffType);
    }

    public override void Revert()
    {
        attack.ShowRange(false);
    }
}
