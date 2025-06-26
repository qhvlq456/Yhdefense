using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public abstract GroundType GetGroundType();
    public abstract void Set(int _idx);
    public abstract void Revert();
}
