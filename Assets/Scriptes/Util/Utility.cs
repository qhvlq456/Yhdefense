using UnityEngine;

public static class Utility
{
    #region Start Object Pool
    public static GameObject GetResObj(PoolingType _type, int _idx)
    {
        GameObject go = null;

        switch (_type)
        {
            case PoolingType.hero:
                go = DataManager.Instance.GetHeroResObj(_idx);
                break;
            case PoolingType.enemy:
                go = DataManager.Instance.GetEnemyResObj(_idx);
                break;
            case PoolingType.heroLand:
                go = DataManager.Instance.GetHeroLandResObj(_idx);
                break;
            case PoolingType.enemyLand:
                go = DataManager.Instance.GetEnemyLandResObj(_idx);
                break;
        }

        return go;
    }

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
        return string.Format("Lv : 1 ,     Ground Type : {0},   Hero Type : {1},   " +
            "Cost : {2},    Attack Speed : {3},     Attack Damage : {4},    Attack Radius : {5}",
            _heroData.groundType, _heroData.heroType, heroUpgradeData.cost, heroUpgradeData.attackSpeed, heroUpgradeData.attackDamage, heroUpgradeData.attackRadius);
    }

    public static bool IsHeroPurchase(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        return GameManager.Instance.gold >= heroUpgradeData.cost;
    }
    #endregion End HeroData

}
