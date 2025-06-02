using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.AI.Navigation;

public class Enemy : Character, IHittable
{
    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    private EnemyAnimation enemyAnimation;

    private bool isDeath;
    public bool IsDeath => isDeath;

    public enum EnemyAnimState { Idle, Walk, Hit, Die }
    private EnemyAnimState currentAnimState = EnemyAnimState.Idle;

    public override void Set(int _idx)
    {
        isDeath = false;
        enemyData = DataManager.Instance.GetIdxToEnemyData(_idx);
        
        move.Initialize(new MoveData(enemyData));
        health.ResetHealth(enemyData.maxHealth);

        enemyAnimation.ChangeState(CharacterAnimation.AnimationState.Walk);
    }

    public void TakeDamage(float _float)
    {
        health.TakeDamage(_float);
        UIManager.Instance.ShowMultipleUI<DmgHUD>(UIPanelType.DmgText).StartDmg((int)_float, transform);

        if (health.currentHealth <= 0)
        {
            Death();
            enemyAnimation.ChangeState(CharacterAnimation.AnimationState.Death);
        }
        else
        {
            enemyAnimation.ChangeState(CharacterAnimation.AnimationState.Hit);
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
    // 먼저 death 처리함으로써 타겟으로 부터 벗어남
    public void Death()
    {
        isDeath = true;
        Revert();
    }
    // animation event 즉, 애니메이션 끝나는것을 대기 하여 회수함
    public void OnDeath()
    {
        ObjectPoolManager.Instance.Retrieve(PoolingType.enemy, enemyData.index, transform);
    }
    public override void Revert()
    {
        Debug.LogError($"enemy is isDeath : {isDeath}");
        move.Revert();
        health.Revert();
    }

    public override GroundType GetGroundType() => enemyData.groundType;

    public Transform GetTransform()
    {
        return transform;
    }

}
