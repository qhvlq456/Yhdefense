using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultyAttack : Attack
{
    private List<IHittable> targetList = new List<IHittable>();

    public override void Execute(float _attackDamage, float _attackSpeed)
    {
        // 쿨타임 갱신
        attackDelay = _attackSpeed > 0 ? 1.0f / _attackSpeed : float.MaxValue;
        SetDelay();

        // 항상 새로 타겟을 찾음
        List<IHittable> targets = FindTargetsInRange();

        if (targets.Count > 0)
        {
            // 여러 타겟을 선택
            targetList = targets
                .OrderBy(hittable => IsTargetInRange(hittable))
                .Take(heroUpgradeData.targetCount)
                .ToList();

            Shoot(targetList, _attackDamage);
        }
        else
        {
            targetList = null;
        }
    }

    private void Shoot(List<IHittable> _targets, float _attackDamage)
    {
        WeaponData weaponData = DataManager.Instance.GetHeroIdxToWeaponData(heroUpgradeData.weaponIdx);

        for (int i = 0; i < _targets.Count; i++)
        {
            IHittable target = _targets[i];

            Bullet bullet = ObjectPoolManager.Instance.Create(PoolingType.weapon, weaponData.index).GetComponent<Bullet>();
            bullet.transform.position = transform.position;

            bullet.Set(weaponData, target,
            (_) =>
            {
                // 버프 등 계산식은 Hero에서 전달된 _attackDamage 사용
                _.TakeDamage(_attackDamage);
            });
        }
    }

    public override void Revert()
    {
        base.Revert();
        targetList = null;
    }
}
