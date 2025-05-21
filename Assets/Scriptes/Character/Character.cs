using System.Reflection;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    // start enemy가 필요한것들
    [SerializeField]
    protected Health health;

    [SerializeField]
    protected Move move;
    // end 

    // start hero가 필요할것
    [SerializeField]
    protected Attack attack;

    [SerializeField]
    protected Buff buff;
    // end

    public abstract GroundType GetGroundType();
    public abstract void Create(int _idx);
    public abstract void Retrieve();
}
