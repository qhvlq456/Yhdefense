using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferHero : Hero
{
    [SerializeField]
    private Buff buff;

    public override void Set(int _idx)
    {
        base.Set(_idx);
        // 필요시 buff 관련 추가 세팅
    }

    public override void SetPreview(int _idx)
    {
        base.SetPreview(_idx);
        // 필요시 buff 관련 미리보기 세팅
    }

    protected override void OnAfterUpgrade()
    {
        // 업그레이드 후 buff 관련 추가 처리 필요시 구현
    }

    private void Update()
    {
        // 버프 범위 내 아군에게 버프 적용
        if (buff != null)
        {
            buff.OnUpdateBuff();
        }
    }
}
