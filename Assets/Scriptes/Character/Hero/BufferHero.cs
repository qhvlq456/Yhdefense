using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// buff의 주체는 Attack hero로써 AttackHero가 BufferHero가 갖고 있는 buffdata를 갖고 있음
/// BufferHero는 단지 BuffData를 관리하는 역활을 함
/// </summary>
public class BufferHero : Hero
{
    [SerializeField]
    private Buff buff;
    public override void Set(int _idx)
    {
        base.Set(_idx);
        // 필요시 buff 관련 추가 세팅
        BuffData buffData = DataManager.Instance.GetBuffData(heroData.heroTypeByIdx, lv);
        buff.Set(buffData, heroData.groundType);
        Debug.LogError($"[Buffer Hero][Set] , upgrade complite after lv : {lv}");
    }

    public override void SetPreview(int _idx)
    {
        base.SetPreview(_idx);
        // 필요시 buff 관련 미리보기 세팅
    }

    protected override void OnAfterUpgrade()
    {
        BuffData buffData = DataManager.Instance.GetBuffData(heroData.heroTypeByIdx, lv);
        buff.Set(buffData, heroData.groundType);
        Debug.LogError($"[Buffer Hero][OnAfterUpgrade] , upgrade complite after lv : {lv}");
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
