using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CharacterManager : Singleton<CharacterManager>
{
    private List<Character> instanceCharacterList = new List<Character>();
    public Hero CreateHero(int _idx, bool _isPreview = false)
    {
        var hero = ObjectPoolManager.Instance.Create(PoolingType.hero, _idx).GetComponent<Hero>();

        if(_isPreview)
        {
            hero.SetPreview(_idx);
        }
        else
        {
            hero.Set(_idx);
        }

        instanceCharacterList.Add(hero);
        return hero;
    }
    // spawn 및 건설 현장 
    public void SpawnEnemy(int _idx, Vector3 _spawnPos, Vector3 _targetPos)
    {
        Debug.LogError($"[CharacterManager] [SpawnEnemy] idx : {_idx}, _spawnPos : {_spawnPos} _targetPos : {_targetPos} ");
        var enemy = ObjectPoolManager.Instance.Create(PoolingType.enemy, _idx).GetComponent<Enemy>();
        // navmeshagent때문에 순서 중요 agent.enabled = false; -> position 설정 -> enabled = true; 순서 중요!
        enemy.Spawn(_spawnPos, _targetPos);
        enemy.Set(_idx);
        instanceCharacterList.Add(enemy);
    }

    public void AllClearCharacter()
    {
        for (int i = 0; i < instanceCharacterList.Count; i++)
        {
            instanceCharacterList[i].Revert();
        }
    }
    
    public bool RemoveCharacter()
    {

        return false;
    }
    // 후에 정말 생각해서 만들기..
    public void SampleCharacter()
    {

    }
}
