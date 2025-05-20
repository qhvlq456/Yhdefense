using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterManager : Singleton<CharacterManager>
{
    private List<Character> instanceCharacterList = new List<Character>();
    public Hero CreateHero(int _idx)
    {
        var hero = ObjectPoolManager.Instance.Create(PoolingType.hero, _idx).GetComponent<Hero>();
        hero.Create(_idx);
        instanceCharacterList.Add(hero);
        return hero;
    }
    // spawn 및 건설 현장 
    public void SpawnEnemy(int _idx, Vector3 _spawnPos, Vector3 _targetPos)
    {
        var enemy = ObjectPoolManager.Instance.Create(PoolingType.enemy, _idx).GetComponent<Enemy>();
        // navmeshagent때문에 순서 중요 agent.enabled = false; → position 설정 → enabled = true; 순서 중요!
        enemy.Spawn(_spawnPos, _targetPos);
        enemy.Create(_idx);
        instanceCharacterList.Add(enemy);
    }

    public void ClearCharacter()
    {
        for (int i = 0; i < instanceCharacterList.Count; i++)
        {
            instanceCharacterList[i].Retrieve();
        }
    }
}
