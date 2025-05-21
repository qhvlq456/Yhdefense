using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private WeaponData weaponData;
    private Action<IHittable> callBack;
    private IHittable target;
    public virtual void Set(WeaponData _weaponData, IHittable _hittable, Action<IHittable> _callBack = null)
    {
        weaponData = _weaponData;
        callBack = _callBack;
        StartCoroutine(CoMove(_hittable.GetTransform().position));
    }
    private IEnumerator CoMove(Vector3 _destination)
    {
        while(Vector3.Distance(transform.position, _destination) > 0.2f)
        {
            transform.position = Vector3.Lerp(transform.position, _destination, weaponData.speed * Time.deltaTime);
            Debug.LogError("213123");
            yield return null;
        }
        transform.position = _destination;
        callBack?.Invoke(target);
        Retrieve();
    }
    public virtual void Retrieve()
    {
        Debug.LogError("Retrieve");
        StopAllCoroutines();
        ObjectPoolManager.Instance.Retrieve(PoolingType.weapon, weaponData.index, transform);
    }
}
