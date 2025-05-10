using UnityEngine;

public static class Utility
{
    public static GameObject GetResObj(PoolingType _type, int _idx)
    {
        GameObject go = null;

        switch (_type)
        {
            case PoolingType.hero:
                go = CharacterManager.Instance.GetHeroResObj(_idx);
                break;
            case PoolingType.enemy:
                go = CharacterManager.Instance.GetEnemyResObj(_idx);
                break;
            case PoolingType.deco:
                break;
            case PoolingType.land:
                go = MapManager.Instance.landResObj;
                break;
        }

        return go;
    }
}
