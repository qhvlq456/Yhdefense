using UnityEngine;
using System.Collections.Generic;

public class Hero : Character, IBuffable
{
    [SerializeField] 
    private HeroData heroData;

    [SerializeField] 
    private HeroUpgradeData upgradeData;

    [SerializeField]
    private int lv;

    public override void Create(int _idx)
    {
        heroData = DataManager.Instance.GetIdxToHeroData(_idx);
        upgradeData = DataManager.Instance.GetHeroUpgradeData(_idx, 1);

        attack.Set(upgradeData, heroData.groundType);
    }

    private void Update()
    {
        if (attack != null)
        {
            // Attack ������Ʈ�� ��Ÿ�� Ÿ�̸� ������Ʈ
            attack.OnUpdateAttack();

            // ������ �������� Ȯ�� �� PerformAttack ȣ��
            if (attack.IsAttack())
            {
                attack.Execute();
            }
        }
    }

    public HeroType GetHeroType => heroData.heroType;
    public override GroundType GetGroundType() => heroData.groundType;

    public void ApplyBuff(float _buffAmount, float _buffDuration, BuffType _buffType)
    {
        throw new System.NotImplementedException();
    }
    public override void Retrieve()
    {
        move.Revert();
        attack.Revert();
        ObjectPoolManager.Instance.Retrieve(PoolingType.hero, heroData.index, transform);
    }

    private void CalculateAttack()
    {

    }

}
