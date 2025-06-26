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
        attack.Set(upgradeData, heroData.groundType);
        InitStats();
    }

    public override void SetPreview(int _idx)
    {
        base.SetPreview(_idx);
        // 미리보기용 추가 세팅 필요시 구현
    }

    protected override void OnAfterUpgrade()
    {
        attack.Set(upgradeData, heroData.groundType);
        RecalculateStats();
    }

    private void Update()
    {
        UpdateBuffs();

        if (attack != null)
        {
            attack.OnUpdateAttack();
            if (attack.IsAttack())
            {
                attack.Execute(currentAttackDamage, currentAttackSpeed);
            }
        }
    }

    private void InitStats()
    {
        currentAttackDamage = upgradeData.attackDamage;
        currentAttackSpeed = upgradeData.attackSpeed;
    }

    private void UpdateBuffs()
    {
        bool recalc = false;
        float now = Time.time;
        for (int i = activeBuffs.Count - 1; i >= 0; i--)
        {
            var buff = activeBuffs[i];
            if (buff.duration > 0 && now - buff.startTime >= buff.duration)
            {
                activeBuffs.RemoveAt(i);
                recalc = true;
            }
        }
        if (recalc)
            RecalculateStats();
    }

    public void ApplyBuff(BuffData buff)
    {
        buff.startTime = Time.time;
        activeBuffs.Add(buff);
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
