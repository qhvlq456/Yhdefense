using UnityEngine;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

public class Attack : MonoBehaviour
{
    [Header("Targeting Options")]
    [SerializeField] 
    private string targetTag = "Enemy"; // �⺻ �±�
    [SerializeField] 
    private LayerMask obstacleMask;     // �þ߸� ������ ������Ʈ��

    [SerializeField]
    protected HeroUpgradeData heroUpgradeData;
    protected GroundType groundType;

    // Time.time ��� ���� ���� ��Ÿ�� ����
    protected float delayTimer; // ���� ���ݱ��� ���� �ð�

    // ���� ���� ���� ������ (��). �� ���� currentCooldownTimer�� ����
    protected float attackDelay;
    protected virtual Color GetGizmoColor() => Color.red;
    public virtual void Set(HeroUpgradeData _heroUpgradeData, GroundType _groundType)
    {
        heroUpgradeData = _heroUpgradeData;
        groundType = _groundType;
        UpdateAttackDelay();
    }
    public virtual void Execute() { }
    // ���� ������ ������Ʈ (attackSpeed ���)
    protected void UpdateAttackDelay()
    {
        if (heroUpgradeData.attackSpeed > 0)
        {
            attackDelay = 1.0f / heroUpgradeData.attackSpeed;
        }
        else
        {
            attackDelay = float.MaxValue; // ���� �Ұ�
            Debug.LogWarning("Attack speed is zero or negative, setting attack delay to infinite.");
        }
    }

    // Update �޼��忡�� ��Ÿ���� ���� (Hero ������Ʈ���� ȣ��)
    // �� �Լ��� Hero ������Ʈ�� Update���� �� ������ ȣ��� ���� �����մϴ�.
    public void OnUpdateAttack()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
        }
    }

    // ���� �������� ���� ��ȯ
    public bool IsAttack()
    {
        return delayTimer <= 0;
    }
    // ���� �� ��Ÿ�� ����
    protected void SetDelay()
    {
        delayTimer = attackDelay;
    }
    // ���� Ž�� �Լ�
    protected List<IHittable> FindTargetsInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, heroUpgradeData.attackRadius);
        List<IHittable> targets = new List<IHittable>();

        foreach (Collider hit in hits)
        {
            // �±� ����
            if (!hit.CompareTag(targetTag))
            {
                continue;
            }

            // IHittable �������̽� üũ
            if (!hit.TryGetComponent(out IHittable hittable))
            {
                continue;
            }

            // GroundType ���� // buff�� ���� hero���� �ؾ߉�
            if (hit.TryGetComponent(out Character character))
            {
                if (character.GetGroundType() != groundType)
                {
                    continue;
                }
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
