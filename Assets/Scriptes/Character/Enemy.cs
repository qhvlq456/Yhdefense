using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.AI.Navigation;


public class Enemy : Character
{
    [SerializeField]
    private EnemyData enemyData;
    public override void Create(int _idx)
    {
        enemyData = DataManager.Instance.GetIdxToEnemyData(_idx);
    }
    public void TakeDamage(float _float)
    {

    }

    public void Spawn(Vector3 _spawnPos, Vector3 _destination)
    {
        Debug.LogError($"_spawnPos : {_spawnPos}, _destination : {_destination}");
        NavMeshHit hit;
        // _spawnPos위치가 NavMesh 위에 있는지 확인
        if (NavMesh.SamplePosition(_spawnPos, out hit, 1f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            StartCoroutine(CoMoveToAfterNavReady(_destination));
        }
        else
        {
            Debug.LogWarning($"NavMesh 위에 위치하지 않음 transform position : {transform.position}");
            return;
        }
    }
    private IEnumerator CoMoveToAfterNavReady(Vector3 _destination)
    {
        yield return null;

        var agent = GetComponent<NavMeshAgent>();
        while (!agent.isOnNavMesh)
        {
            yield return null;
        }

        move.Movement(_destination);
    }

    public override void Retrieve()
    {
        base.Retrieve();
        move.Revert();
        ObjectPoolManager.Instance.Retrieve(PoolingType.enemy, enemyData.index, transform);
    }
}
