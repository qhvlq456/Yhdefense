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

        // 기존 타겟 존재
        if(target != null && IsTargetInRange(target))
        {
            Shoot(target);
        }
        else
        {
            // 새 타겟 찾기
            List<IHittable> targets = FindTargetsInRange();

            if (targets.Count > 0)
            {
                target = targets
                .OrderBy(hittable => Vector3.Distance(transform.position, hittable.GetTransform().position))
                .FirstOrDefault(); // 나와의 거리순 정렬후 가장 가까운 타겟을 선택

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
                // 후에 계산식 buff 등등 통일하여 들어갈 것
                _.TakeDamage(heroUpgradeData.attackDamage);
            }
        });
    }
    private bool IsTargetInRange(IHittable _target)
    {
        // 타겟의 Transform이 유효한지 확인
        if (_target.GetTransform() == null)
        {
            return false;
        }

        // 타겟이 범위 내에 있는지 확인
        return Vector3.Distance(transform.position, _target.GetTransform().position) <= heroUpgradeData.attackRadius;
    }
    // Attack 클래스의 Revert 오버라이드 (추가적인 정리 로직이 필요할 경우)
    public override void Revert()
    {
        base.Revert(); // 부모 Revert 호출
        target = null; // 현재 타겟 초기화
    }
}
