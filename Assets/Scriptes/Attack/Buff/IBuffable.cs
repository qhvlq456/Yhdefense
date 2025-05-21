public interface IBuffable
{
    /// <summary>
    /// 버프를 적용합니다.
    /// </summary>
    /// <param name="buffAmount">버프의 효과량 (예: 공격력 증가량, 이동 속도 감소량 등)</param>
    /// <param name="buffDuration">버프의 지속 시간 (선택 사항)</param>
    /// <param name="buffType">버프의 종류 (열거형 등으로 구분)</param>
    void ApplyBuff(float _buffAmount, float _buffDuration, BuffType _buffType);

    // 필요하다면 추가적인 함수를 정의할 수 있습니다.
    // 예: void RemoveBuff(BuffType buffType);
    // 예: bool HasBuff(BuffType buffType);
}