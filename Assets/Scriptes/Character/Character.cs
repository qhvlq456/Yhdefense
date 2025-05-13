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

    protected CharacterData data;
    
    public virtual void Create(CharacterData _data)
    {
        data = _data;
    }
    public virtual void Retrieve()
    {
        
    }
}
