using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Hero : Character, IBuffable
{
    [SerializeField] 
    private HeroData heroData;
    public HeroData HeroData => heroData;

    [SerializeField] 
    private HeroUpgradeData upgradeData;

    [SerializeField]
    private int lv;
    public int Lv => lv;
    public override void Set(int _idx)
    {
        heroData = DataManager.Instance.GetIdxToHeroData(_idx);
        upgradeData = DataManager.Instance.GetHeroUpgradeData(_idx, 1);

        attack.Set(upgradeData, heroData.groundType);
    }

    public void SetPreview(int _idx)
    {
        heroData = DataManager.Instance.GetIdxToHeroData(_idx);
        //upgradeData 데이터가 존재하지 않음으로 공격을 할 수 없음 왜냐 IsAttack의 데이터가 없음
        // attack.Set(upgradeData, heroData.groundType); // 공격 컴포넌트는 초기화하지 않음
        // UpdateAttackDelay 이 함수가 else를 타서 attackDelay가 float.MaxValue로 설정됨
        // 나중에 더 좋은 방법이 있음 탐구 할 것
    }
    private void Update()
    {
        if (attack != null)
        {
            // Attack 컴포넌트의 쿨타임 타이머 업데이트
            attack.OnUpdateAttack();

            // 공격이 가능한지 확인 후 PerformAttack 호출
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
    public override void Revert()
    {
        // 후에 null 체크들 없앨 것
        if(move != null)
        {
            move.Revert();
        }

        if(attack != null)
        {
            attack.Revert();
        }

        ObjectPoolManager.Instance.Retrieve(PoolingType.hero, heroData.index, transform);
    }

    private void CalculateAttack()
    {

    }

    public void UpgradeHero()
    {
        if(Utility.IsHeroUpgrade(heroData, lv + 1))
        {
            Utility.UpgradeHero(heroData, lv + 1);
            lv++;
            upgradeData = DataManager.Instance.GetHeroUpgradeData(heroData.index, lv);
            attack.Set(upgradeData, heroData.groundType);
        }
        else
        {
            Debug.Log("업그레이드 할 수 없습니다.");
        }
    }
    public void SellHero()
    {
        Revert();
        Utility.GetHeroSellPrice(heroData, lv);
    }
}
