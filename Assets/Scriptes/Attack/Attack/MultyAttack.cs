using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultyAttack : Attack
{
    private List<IHittable> targetList = new List<IHittable>();
    public override void Execute()
    {
        base.Execute();

        // 항상 새로 타겟을 찾음
        List<IHittable> targets = FindTargetsInRange();

        if (targets.Count > 0)
        {
            targetList = targets
                .OrderBy(hittable => IsTargetInRange(hittable))
                .Take(heroUpgradeData.targetCount)
                .ToList();

            Shoot(targetList);
        }
        else
        {
            targetList = null;
        }
    }

    private void Shoot(List<IHittable> _targets)
    {
        SetDelay();

        WeaponData weaponData = DataManager.Instance.GetHeroIdxToWeaponData(heroUpgradeData.weaponIdx);

        for(int i = 0; i < _targets.Count; i++)
        {
            IHittable target = _targets[i];

            Bullet bullet = ObjectPoolManager.Instance.Create(PoolingType.weapon, weaponData.index).GetComponent<Bullet>();
            // 후에 변경
            bullet.transform.position = transform.position;

            bullet.Set(weaponData, _targets[i],
            (_) =>
            {
                Debug.LogError($"{i} : callback");
                // 후에 계산식 buff 등등 통일하여 들어갈 것
                _.TakeDamage(heroUpgradeData.attackDamage);
            });
        }
    }

    public override void Revert()
    {
        base.Revert(); // 부모 Revert 호출
        targetList = null; // 현재 타겟 초기화
    }
}
