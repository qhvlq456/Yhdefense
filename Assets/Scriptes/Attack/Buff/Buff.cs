using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [Header("Targeting Options")]
    [SerializeField]
    private string targetTag = "Hero"; // �⺻ �±�
    [SerializeField]
    private LayerMask obstacleMask;

    protected virtual float GetBuffRadius() => float.MinValue;
    protected virtual Color GetGizmoColor() => Color.red;

    // ���� Ž�� �Լ�
    protected List<IBuffable> FindTargetsInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, GetBuffRadius());
        List<IBuffable> targets = new List<IBuffable>();

        foreach (Collider hit in hits)
        {
            // �±� ����
            if (!hit.CompareTag(targetTag))
            {
                continue;
            }

            // IHittable �������̽� üũ
            if (!hit.TryGetComponent(out IBuffable hittable))
            {
                continue;
            }


            // ��ֹ� üũ (Raycast �þ� ����)
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
