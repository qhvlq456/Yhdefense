using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterManager : Singleton<CharacterManager>
{
    public GameObject CreateHero()
    {
        return null;
    }
    // spawn 및 건설 현장 
    public GameObject SpawnEnemy(CharacterData _data, Vector3 _spawnPos, Vector3 _targetPos)
    {
        var enemy = ObjectPoolManager.Instance.Create(PoolingType.enemy, _data.index).GetComponent<Enemy>();
        enemy.Create(_data);
        enemy.Spawn(_spawnPos, _targetPos);
        return enemy.gameObject;
    }
}
