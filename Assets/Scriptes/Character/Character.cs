using UnityEngine;

public class Character<T> : MonoBehaviour where T : struct
{
    [SerializeField]
    protected Health health;

    [SerializeField]
    protected Move move;

    [SerializeField]
    protected Attack attack;

    protected T data;
    
    public virtual void Create(T _data)
    {
        data = _data;
    }
    public virtual void Retrieve()
    {
        
    }
}
