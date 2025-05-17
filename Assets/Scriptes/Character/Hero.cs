using UnityEngine;
using System.Collections.Generic;

public class Hero : Character
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
        upgradeData = DataManager.Instance.GetHeroUpgradeData(_idx, lv);
    }

    

    public Enemy FindTarget()
    {
        // TODO: Å¸°Ù Å½»ö ·ÎÁ÷
        return null;
    }


    public override void Retrieve()
    {
        base.Retrieve();
        move.Revert();
        ObjectPoolManager.Instance.Retrieve(PoolingType.hero, heroData.index, transform);
    }
}
