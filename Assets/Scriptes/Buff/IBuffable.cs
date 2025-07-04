public interface IBuffable
{
    void ApplyBuff(BuffData _buff);
    void RemoveBuff(BuffType _buffType);
    bool HasBuff(BuffType _buffType);
}