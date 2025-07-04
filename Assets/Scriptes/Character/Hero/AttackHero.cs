using UnityEngine;
using System.Collections.Generic;

public class AttackHero : Hero, IBuffable
{
    [SerializeField]
    protected Attack attack;

    private List<BuffData> activeBuffs = new();
    private float currentAttackDamage;
    private float currentAttackSpeed;

    public override void Set(int _idx)
    {
        base.Set(_idx);
        AttackData attackData = DataManager.Instance.GetAttackData(heroData.index, lv);
        attack.Set(attackData, heroData.groundType);
        InitStats();
    }

    public override void SetPreview(int _idx)
    {
        base.SetPreview(_idx);
        // 미리보기용 추가 세팅 필요시 구현
    }

    protected override void OnAfterUpgrade()
    {
        AttackData attackData = DataManager.Instance.GetAttackData(heroData.index, lv);
        attack.Set(attackData, heroData.groundType);
        Debug.LogError($"upgrade complite after lv : {lv}");
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

    public void ApplyBuff(BuffData _buff)
    {
        _buff.startTime = Time.time;
        activeBuffs.Add(_buff);
        RecalculateStats();
    }

    public void RemoveBuff(BuffType buffType)
    {
        activeBuffs.RemoveAll(b => b.buffType == buffType);
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
                // 필요시 추가
            }
        }
    }

    public bool HasBuff(BuffType buffType)
    {
        return activeBuffs.Exists(b => b.buffType == buffType);
    }
}
