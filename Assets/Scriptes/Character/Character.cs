using System.Reflection;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected Health health;

    [SerializeField]
    protected Move move;

    [SerializeField]
    protected Attack attack;

    public abstract GroundType GetGroundType();
    public abstract void Create(int _idx);
    public abstract void Retrieve();
}
