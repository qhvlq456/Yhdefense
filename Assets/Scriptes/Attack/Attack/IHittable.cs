using UnityEngine;

public interface IHittable
{
    public void TakeDamage(float _dmg);
    public Transform GetTransform();
}
