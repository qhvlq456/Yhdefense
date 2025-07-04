using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Hero : Character
{
    [SerializeField]
    protected HeroData heroData;
    public HeroData HeroData => heroData;

    [SerializeField]
    protected HeroUpgradeData upgradeData;

    [SerializeField]
    protected int lv;
    public int Lv => lv;

    // 파생 클래스에서 필요한 컴포넌트(Attack, Buff 등)는 각자 선언

    public override void Set(int _idx)
    {
        lv = 1;
        heroData = DataManager.Instance.GetIdxToHeroData(_idx);
        upgradeData = DataManager.Instance.GetHeroUpgradeData(_idx, lv);
        OnAfterSet();
    }

    /// <summary>
    /// 미리보기용 데이터 세팅(파생에서 필요시 오버라이드)
    /// </summary>
    public virtual void SetPreview(int _idx)
    {
        heroData = DataManager.Instance.GetIdxToHeroData(_idx);
        // upgradeData는 미리보기에서 필요시 파생에서 처리
    }

    /// <summary>
    /// 파생 클래스에서 Set 이후 추가 세팅이 필요할 때 오버라이드
    /// </summary>
    protected virtual void OnAfterSet() { }

    public HeroType GetHeroType => heroData.heroType;
    public override GroundType GetGroundType() => heroData.groundType;

    public override void Revert()
    {
        ObjectPoolManager.Instance.Retrieve(PoolingType.hero, heroData.index, transform);
    }

    /// <summary>
    /// 업그레이드(파생에서 스탯 재계산, 컴포넌트 갱신 등 추가 구현)
    /// </summary>
    public virtual void UpgradeHero()
    {
        if (Utility.IsHeroUpgrade(heroData, lv + 1))
        {
            Debug.LogError($"upgrade complite before lv : {lv}");
            Utility.UpgradeHero(heroData, lv + 1);
            lv++;
            upgradeData = DataManager.Instance.GetHeroUpgradeData(heroData.index, lv);
            OnAfterUpgrade();
        }
        else
        {
            Debug.Log("업그레이드 할 수 없습니다.");
        }
    }

    /// <summary>
    /// 업그레이드 후 파생에서 추가 처리 필요시 오버라이드
    /// </summary>
    protected virtual void OnAfterUpgrade() { }

    /// <summary>
    /// 판매(파생에서 추가 처리 필요시 오버라이드)
    /// </summary>
    public virtual void SellHero()
    {
        Revert();
        Utility.GetHeroSellPrice(heroData, lv);
    }
}
