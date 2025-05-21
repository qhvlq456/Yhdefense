using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [Header("Targeting Options")]
    [SerializeField]
    private string targetTag = "Hero"; // 기본 태그
    [SerializeField]
    private LayerMask obstacleMask;

    protected virtual float GetBuffRadius() => float.MinValue;
    protected virtual Color GetGizmoColor() => Color.red;

    // 공통 탐색 함수
    protected List<IBuffable> FindTargetsInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, GetBuffRadius());
        List<IBuffable> targets = new List<IBuffable>();

        foreach (Collider hit in hits)
        {
            // 태그 필터
            if (!hit.CompareTag(targetTag))
            {
                continue;
            }

            // IHittable 인터페이스 체크
            if (!hit.TryGetComponent(out IBuffable hittable))
            {
                continue;
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

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = GetGizmoColor();
        Gizmos.DrawWireSphere(transform.position, GetBuffRadius());
    }
#endif
}
