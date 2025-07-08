using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class LandEnemy : Enemy
{
    public override void Set(int _idx)
    {
        base.Set(_idx);
    }
    public override void Spawn(Vector3 _spawnPos, Vector3 _destination)
    {
        base.Spawn(_spawnPos, _destination);
        NavMeshHit hit;
        // _spawnPos위치가 NavMesh 위에 있는지 확인
        if (NavMesh.SamplePosition(_spawnPos, out hit, 1f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            StartCoroutine(CoMoveToAfterNavReady(_destination));
        }
        else
        {
            Debug.LogWarning($"[LandEnemy] NavMesh 위에 위치하지 않음 transform position : {transform.position}");
            return;
        }
    }
    private IEnumerator CoMoveToAfterNavReady(Vector3 _destination)
    {
        // agent spawn 후 0.1초 후 동작 시작 왜냐 navmeshobstacle 이 반영이 안되어서
        yield return new WaitForSeconds(0.1f);

        var agent = GetComponent<NavMeshAgent>();

        while (!agent.isOnNavMesh)
        {
            Debug.Log("[LandEnemy] 아직 NavMesh 위에 없음");
            yield return null;
        }

        Debug.Log("[LandEnemy] NavMesh 위에 올라감");
        move.Movement(_destination);
    }
}
