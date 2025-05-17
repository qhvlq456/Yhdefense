using UnityEngine;
using System.Collections.Generic;

public abstract class Attack : MonoBehaviour
{
    [Header("Targeting Options")]
    [SerializeField] private string targetTag = "Enemy"; // 기본 태그
    [SerializeField] private LayerMask obstacleMask;     // 시야를 가리는 오브젝트용

    // 자식이 반지름, 색상 제공
    protected abstract float GetAttackRadius();
    protected abstract GroundType GetGroundType(); // 지형 타입 확인용
    protected virtual Color GetGizmoColor() => Color.red;

    // 공통 탐색 함수
    protected List<IHittable> FindTargetsInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, GetAttackRadius());
        List<IHittable> targets = new();

        foreach (Collider hit in hits)
        {
            // 태그 필터
            if (!hit.CompareTag(targetTag)) continue;

            // IHittable 인터페이스 체크
            if (!hit.TryGetComponent(out IHittable hittable)) continue;

            // GroundType 필터
            if (hit.TryGetComponent(out Character character))
            {
                if (character.GetGroundType() != GetGroundType())
                    continue;
            }

            // 장애물 체크 (Raycast 시야 차단)
            Vector3 dir = (hit.transform.position - transform.position).normalized;
            float dist = Vector3.Distance(transform.position, hit.transform.position);
            if (Physics.Raycast(transform.position, dir, dist, obstacleMask))
                continue;

            targets.Add(hittable);
        }

        return targets;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = GetGizmoColor();
        Gizmos.DrawWireSphere(transform.position, GetAttackRadius());
    }
#endif
}
