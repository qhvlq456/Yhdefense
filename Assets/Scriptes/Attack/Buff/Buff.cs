using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    [Header("Targeting Options")]
    [SerializeField]
    private string targetTag = "Hero";
    [SerializeField]
    private LayerMask obstacleMask;

    [Header("Buff Options")]
    [SerializeField]
    private BuffData buffData;
    protected HeroUpgradeData heroUpgradeData;
    // 지상 버퍼냐 공중 버퍼냐 둘다냐
    protected GroundType groundType;

    [SerializeField]
    private float buffInterval = 1f; // 버프 적용 주기(초)

    private float timer;

    // BuffData의 range를 항상 반영
    protected virtual float GetBuffRadius() => buffData.range > 0 ? buffData.range : 3f;
    protected virtual Color GetGizmoColor() => Color.cyan;
    // set에 추가할 것
    // HeroUpgradeData _heroUpgradeData, GroundType _groundType
    public virtual void Set(BuffData _buffData)
    {
        buffData = _buffData;
    }

    // 주기적으로 버프 적용
    public void OnUpdateBuff()
    {
        timer += Time.deltaTime;
        if (timer >= buffInterval)
        {
            timer = 0f;
            ApplyBuffToTargets();
        }
    }

    protected void ApplyBuffToTargets()
    {
        var targets = FindTargetsInRange();
        foreach (var target in targets)
        {
            // 중복 적용 방지: 이미 같은 타입의 버프가 있으면 startTime만 갱신
            if (target.HasBuff(buffData.buffType))
            {
                target.RemoveBuff(buffData.buffType);
            }
            BuffData applyData = buffData;
            applyData.startTime = Time.time;
            target.ApplyBuff(applyData);
        }
    }

    // 공통 탐색 함수
    protected List<IBuffable> FindTargetsInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, GetBuffRadius());
        List<IBuffable> targets = new List<IBuffable>();

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag(targetTag)) continue;
            if (!hit.TryGetComponent(out IBuffable buffable)) continue;

            Vector3 dir = (hit.transform.position - transform.position).normalized;
            float dist = Vector3.Distance(transform.position, hit.transform.position);

            if (Physics.Raycast(transform.position, dir, dist, obstacleMask)) continue;

            targets.Add(buffable);
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
