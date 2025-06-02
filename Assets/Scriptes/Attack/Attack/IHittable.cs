using UnityEngine;

public interface IHittable
{
    public void TakeDamage(float _dmg);
    public bool IsDeath { get; }
    public Transform GetTransform();
}
