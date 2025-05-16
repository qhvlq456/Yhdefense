using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]
public class HeroLand : Land
{
    [SerializeField]
    private NavMeshObstacle navObstacle;
    [SerializeField]
    private Transform head;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
