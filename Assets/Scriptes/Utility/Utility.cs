using UnityEngine;

public static class Utility
{
    #region Static Variables
    public readonly static int HERO_MAX_LV = 3;
    #endregion
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
    public static string GetHeroInfo(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        string infoText = string.Empty;

        if (_lv <= DataManager.Instance.GetMaxHeroUpgradeLevel(_heroData.index))
        {
            infoText = $"Lv : {_lv}\n" +
                       $"이름 : {_heroData.name}\n" +
                       $"공격타입 : {_heroData.groundType}\n" +
                       $"영웅타입 : {_heroData.heroType}\n" +
                       $"비용 : {heroUpgradeData.cost}\n";

            switch (_heroData.heroType)
            {
                case HeroType.Attack:
                    {
                        // 공격형 히어로
                        AttackData attackData = DataManager.Instance.GetAttackData(_heroData.heroTypeByIdx, _lv);
                        infoText += $"공격속도 : {attackData.speed}\n" +
                                    $"공격력 : {attackData.damage}\n" +
                                    $"공격범위 : {attackData.radius}\n";
                        break;
                    }
                case HeroType.Buffer:
                    {
                        // 버퍼형 히어로
                        BuffData buffData = DataManager.Instance.GetBuffData(_heroData.heroTypeByIdx, _lv);
                        infoText += $"버프타입 : {buffData.buffType}\n" +
                                    $"버프효과 : {buffData.amount}\n" +
                                    $"버프범위 : {buffData.range}\n" +
                                    $"버프지속 : {buffData.duration}\n";
                        break;
                    }
                case HeroType.Debuffer:
                    {
                        // 디버퍼형 히어로
                        DebuffData debuffData = DataManager.Instance.GetDebuffData(_heroData.heroTypeByIdx, _lv);
                        infoText += $"디버프타입 : {debuffData.debuffType}\n" +
                                    $"디버프효과 : {debuffData.amount}\n" +
                                    $"디버프범위 : {debuffData.range}\n" +
                                    $"디버프지속 : {debuffData.duration}\n";
                        break;
                    }
            }

            infoText += $"판매가격 : {heroUpgradeData.sell}";
        }
        else
        {
            infoText = "Max Upgrade!!";
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
        return GameManager.Instance.gold >= heroUpgradeData.cost && _lv > HERO_MAX_LV;
    }
    public static void UpgradeHero(HeroData _heroData, int _lv = 1)
    {
        HeroUpgradeData heroUpgradeData = DataManager.Instance.GetHeroUpgradeData(_heroData.index, _lv);
        GameManager.Instance.UpdateGold(-heroUpgradeData.cost);
        // 후에 업그레이드 로직 추가
    }

    #endregion End HeroData
}
