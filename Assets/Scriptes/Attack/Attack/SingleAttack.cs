using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SingleAttack : Attack
{
    [SerializeField]
    private GameObject bulletRes;

    private IHittable target;
    public override void Execute()
    {
        base.Execute();

        // ���� Ÿ�� ����
        if(target != null && IsTargetInRange(target))
        {
            Shoot(target);
        }
        else
        {
            // �� Ÿ�� ã��
            List<IHittable> targets = FindTargetsInRange();

            if (targets.Count > 0)
            {
                target = targets
                .OrderBy(hittable => Vector3.Distance(transform.position, hittable.GetTransform().position))
                .FirstOrDefault(); // ������ �Ÿ��� ������ ���� ����� Ÿ���� ����

                if (target != null)
                {
                    Shoot(target);
                }
            }
            else
            {
                target = null;
            }
        }
    }
    private void Shoot(IHittable _target)
    {
        SetDelay();
        Bullet bullet = Instantiate(bulletRes, transform.position, Quaternion.identity).GetComponent<Bullet>();

        WeaponData weaponData = DataManager.Instance.GetHeroIdxToWeaponData(heroUpgradeData.heroIdx);
        bullet.Set(weaponData, _target,
            (_) =>
        {
            if(_ != null)
            {
                Debug.LogError("callback");
                // �Ŀ� ���� buff ��� �����Ͽ� �� ��
                _.TakeDamage(heroUpgradeData.attackDamage);
            }
        });
    }
    private bool IsTargetInRange(IHittable _target)
    {
        // Ÿ���� Transform�� ��ȿ���� Ȯ��
        if (_target.GetTransform() == null)
        {
            return false;
        }

        // Ÿ���� ���� ���� �ִ��� Ȯ��
        return Vector3.Distance(transform.position, _target.GetTransform().position) <= heroUpgradeData.attackRadius;
    }
    // Attack Ŭ������ Revert �������̵� (�߰����� ���� ������ �ʿ��� ���)
    public override void Revert()
    {
        base.Revert(); // �θ� Revert ȣ��
        target = null; // ���� Ÿ�� �ʱ�ȭ
    }
}
