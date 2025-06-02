using UnityEngine;

public static class Utility
{
    #region Start Object Pool

    public static PoolingType LandTypeToPoolingType(LandType _type)
    {
        PoolingType type = PoolingType.heroLand;

        switch (_type)
        {
            case LandType.hero:
                type = PoolingType.heroLand;
                break;
            case LandType.enemy:
                type = PoolingType.enemyLand;
                break;
        }

        return type;
    }
    #endregion End Object Pool

    #region Start HeroData
    // 후에 버퍼 및 디버퍼들의 인포들을 타입별로 나누어 보낼것
    public static string GetHeroInfo(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        string infoText = string.Empty;

        if (_lv <= DataManager.Instance.GetMaxHeroUpgradeLevel(_heroData.index))
        {
            infoText = string.Format("Lv : {0} , \n 공격타입 : {1}, \n 영웅타입 : {2}, \n" +
            "비용 : {3}, \n 공격속도 : {4}, \n 공격력 : {5}, \n공격범위 : {6} \n 판매가격 : {7}",
            _lv,
            _heroData.groundType,
            _heroData.heroType,
            heroUpgradeData.cost,
            heroUpgradeData.attackSpeed,
            heroUpgradeData.attackDamage,
            heroUpgradeData.attackRadius,
            heroUpgradeData.sell
            );
        }
        else
        {
            infoText = string.Format("Max Upgrade!!");
        }

        return infoText;
    }

    public static bool IsHeroPurchase(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        return GameManager.Instance.gold >= heroUpgradeData.cost;
    }
    public static void PurchaseHero(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        GameManager.Instance.UpdateGold(-heroUpgradeData.cost);
        // 후에 구매 로직 추가
    }
    public static void GetHeroSellPrice(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        GameManager.Instance.UpdateGold(heroUpgradeData.sell);
    }
    public static bool IsHeroUpgrade(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        return GameManager.Instance.gold >= heroUpgradeData.cost;
    }
    public static void UpgradeHero(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        GameManager.Instance.UpdateGold(-heroUpgradeData.cost);
        // 후에 업그레이드 로직 추가
    }

    #endregion End HeroData
}
