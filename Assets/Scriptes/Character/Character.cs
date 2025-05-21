using System.Reflection;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    // start enemy�� �ʿ��Ѱ͵�
    [SerializeField]
    protected Health health;

    [SerializeField]
    protected Move move;
    // end 

    // start hero�� �ʿ��Ұ�
    [SerializeField]
    protected Attack attack;

    [SerializeField]
    protected Buff buff;
    // end

    public abstract GroundType GetGroundType();
    public abstract void Create(int _idx);
    public abstract void Retrieve();
}
