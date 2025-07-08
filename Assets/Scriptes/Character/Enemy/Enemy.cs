using UnityEngine;
using System.Collections;

public class Enemy : Character, IHittable
{
    // start enemy가 필요한것들
    [SerializeField]
    protected Health health;

    [SerializeField]
    protected Move move;

    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    protected EnemyAnimation enemyAnimation;

    private bool isDeath;
    public bool IsDeath => isDeath;

    public override void Set(int _idx)
    {
        isDeath = false;
        enemyData = DataManager.Instance.GetIdxToEnemyData(_idx);
        Debug.LogError($"[Enemy] Set enemyData : {enemyData.name}, groundtype : {enemyData.groundType}");
        move.Initialize(DataManager.Instance.GetMoveData(_idx));
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

    public virtual void Spawn(Vector3 _spawnPos, Vector3 _destination)
    {
        Debug.LogError($"_spawnPos : {_spawnPos}, _destination : {_destination}");
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
        OnDeath();
    }

    public override GroundType GetGroundType() => enemyData.groundType;

    public Transform GetTransform()
    {
        return transform;
    }

}
