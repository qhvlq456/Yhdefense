using UnityEngine;

public static class Utility
{
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
}
