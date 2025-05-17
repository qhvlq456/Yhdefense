using UnityEngine;
using System.Collections.Generic;

public abstract class Attack : MonoBehaviour
{
    [Header("Targeting Options")]
    [SerializeField] private string targetTag = "Enemy"; // �⺻ �±�
    [SerializeField] private LayerMask obstacleMask;     // �þ߸� ������ ������Ʈ��

    // �ڽ��� ������, ���� ����
    protected abstract float GetAttackRadius();
    protected abstract GroundType GetGroundType(); // ���� Ÿ�� Ȯ�ο�
    protected virtual Color GetGizmoColor() => Color.red;

    // ���� Ž�� �Լ�
    protected List<IHittable> FindTargetsInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, GetAttackRadius());
        List<IHittable> targets = new();

        foreach (Collider hit in hits)
        {
            // �±� ����
            if (!hit.CompareTag(targetTag)) continue;

            // IHittable �������̽� üũ
            if (!hit.TryGetComponent(out IHittable hittable)) continue;

            // GroundType ����
            if (hit.TryGetComponent(out Character character))
            {
                if (character.GetGroundType() != GetGroundType())
                    continue;
            }

            // ��ֹ� üũ (Raycast �þ� ����)
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
