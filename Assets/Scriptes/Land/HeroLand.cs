using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class HeroLand : Land
{
    [SerializeField]
    private NavMeshObstacle navObstacle;
    [SerializeField]
    private Transform head;

    public void CreateHero()
    {
        // open ui
        // callback �޾ƾ���
    }
    
    public void SetHero(Hero _hero)
    {
        if(head == null)
        {
            
        }
    }
    public void RemoveHero(Hero _hero) 
    { 

    }
}
