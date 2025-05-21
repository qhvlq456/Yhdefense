using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class Attack : MonoBehaviour
{
    [Header("Targeting Options")]
    [SerializeField] 
    private string targetTag = "Enemy"; // 기본 태그
    [SerializeField] 
    private LayerMask obstacleMask;     // 시야를 가리는 오브젝트용

    [SerializeField]
    protected HeroUpgradeData heroUpgradeData;
    protected GroundType groundType;

    // Time.time 대신 사용될 남은 쿨타임 변수
    protected float delayTimer; // 다음 공격까지 남은 시간

    // 계산된 실제 공격 딜레이 (초). 이 값을 currentCooldownTimer에 대입
    protected float attackDelay;
    protected virtual Color GetGizmoColor() => Color.red;
    public virtual void Set(HeroUpgradeData _heroUpgradeData, GroundType _groundType)
    {
        heroUpgradeData = _heroUpgradeData;
        groundType = _groundType;
        UpdateAttackDelay();
    }
    public virtual void Execute() { }
    // 공격 딜레이 업데이트 (attackSpeed 기반)
    protected void UpdateAttackDelay()
    {
        if (heroUpgradeData.attackSpeed > 0)
        {
            attackDelay = 1.0f / heroUpgradeData.attackSpeed;
        }
        else
        {
            attackDelay = float.MaxValue; // 공격 불가
            Debug.LogWarning("Attack speed is zero or negative, setting attack delay to infinite.");
        }
    }

    // Update 메서드에서 쿨타임을 관리 (Hero 컴포넌트에서 호출)
    // 이 함수는 Hero 컴포넌트의 Update에서 매 프레임 호출될 것을 가정합니다.
    public void OnUpdateAttack()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
    }

    // 공격 가능한지 여부 반환
    public bool IsAttack()
    {
        return delayTimer <= 0;
    }
    // 공격 후 쿨타임 설정
    protected void SetDelay()
    {
        delayTimer = attackDelay;
    }
    // 공통 탐색 함수
    protected List<IHittable> FindTargetsInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, heroUpgradeData.attackRadius);
        List<IHittable> targets = new List<IHittable>();

        foreach (Collider hit in hits)
        {
            // 태그 필터
            if (!hit.CompareTag(targetTag))
            {
                continue;
            }

            // IHittable 인터페이스 체크
            if (!hit.TryGetComponent(out IHittable hittable))
            {
                continue;
            }

            // GroundType 필터 // buff는 같은 hero끼리 해야됌
            if (hit.TryGetComponent(out Character character))
            {
                if (character.GetGroundType() != groundType)
                {
                    continue;
                }
            }

            // 장애물 체크 (Raycast 시야 차단)
            Vector3 dir = (hit.transform.position - transform.position).normalized;
            float dist = Vector3.Distance(transform.position, hit.transform.position);

            if (Physics.Raycast(transform.position, dir, dist, obstacleMask))
            {
                continue;
            }

            targets.Add(hittable);
        }

        return targets;
    }
    public virtual void Revert()
    {

    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = GetGizmoColor();
        Gizmos.DrawWireSphere(transform.position, heroUpgradeData.attackRadius);
    }
#endif
}
