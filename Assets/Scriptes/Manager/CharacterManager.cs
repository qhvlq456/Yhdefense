using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CharacterManager : Singleton<CharacterManager>
{
    private List<Character> instanceCharacterList = new List<Character>();
    public GameObject CreateHero()
    {
        return null;
    }
    // spawn �� �Ǽ� ���� 
    public void SpawnEnemy(int _idx, Vector3 _spawnPos, Vector3 _targetPos)
    {
        var enemy = ObjectPoolManager.Instance.Create(PoolingType.enemy, _idx).GetComponent<Enemy>();
        enemy.Spawn(_spawnPos, _targetPos);
        enemy.Create(_idx);
        instanceCharacterList.Add(enemy);
        // agent.enabled = false; �� position ���� �� enabled = true; ���� �߿�!
    }

    public void ClearCharacter()
    {
        for (int i = 0; i < instanceCharacterList.Count; i++)
        {
            instanceCharacterList[i].Retrieve();
        }
    }
}
