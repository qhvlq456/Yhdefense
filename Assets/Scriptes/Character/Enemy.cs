using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.AI.Navigation;

public class Enemy : Character, IHittable
{
    [SerializeField]
    private EnemyData enemyData;

    public bool isDie { get; private set; }
    public override void Create(int _idx)
    {
        isDie = false;
        enemyData = DataManager.Instance.GetIdxToEnemyData(_idx);
        
        // 임시 수정할 것
        move.Initialize(new MoveData());
        health.ResetHealth(enemyData.maxHealth);
    }
    public void TakeDamage(float _float)
    {
        health.TakeDamage(_float);

        if (health.currentHealth <= 0)
        {
            Die();
        }
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
            Debug.Log("아직 NavMesh 위에 없음");
            yield return null;
        }

        Debug.Log("NavMesh 위에 올라감");
        move.Movement(_destination);
    }
    private void Die()
    {
        isDie = true;
    }

    public override void Retrieve()
    {
        move.Revert();
        health.ResetHealth(enemyData.maxHealth);
        ObjectPoolManager.Instance.Retrieve(PoolingType.enemy, enemyData.index, transform);
    }

    public override GroundType GetGroundType() => enemyData.groundType;

    public Transform GetTransform()
    {
        return transform;
    }

}
