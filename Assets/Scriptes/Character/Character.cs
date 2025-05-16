using System.Reflection;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    protected Health health;

    [SerializeField]
    protected Move move;

    [SerializeField]
    protected Attack attack;
    
    public virtual void Create(int _idx)
    {
        
    }
    public virtual void Retrieve()
    {
        
    }
}
