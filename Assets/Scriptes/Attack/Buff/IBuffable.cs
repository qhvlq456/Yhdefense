public interface IBuffable
{
    /// <summary>
    /// ������ �����մϴ�.
    /// </summary>
    /// <param name="buffAmount">������ ȿ���� (��: ���ݷ� ������, �̵� �ӵ� ���ҷ� ��)</param>
    /// <param name="buffDuration">������ ���� �ð� (���� ����)</param>
    /// <param name="buffType">������ ���� (������ ������ ����)</param>
    void ApplyBuff(float _buffAmount, float _buffDuration, BuffType _buffType);

    // �ʿ��ϴٸ� �߰����� �Լ��� ������ �� �ֽ��ϴ�.
    // ��: void RemoveBuff(BuffType buffType);
    // ��: bool HasBuff(BuffType buffType);
}