using UnityEngine;
using UnityEngine.AI;

public class Hero : Character
{
    [SerializeField]
    private HeroData heroData;
    public override void Create(int _idx)
    {
        heroData = DataManager.Instance.GetIdxToHeroData(_idx);
    }
    public override void Retrieve()
    {
        base.Retrieve();
        move.Revert();
        ObjectPoolManager.Instance.Retrieve(PoolingType.hero, heroData.index, transform);
    }
}
