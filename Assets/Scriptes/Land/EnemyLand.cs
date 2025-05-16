using Unity.AI.Navigation;
using UnityEngine;

public class EnemyLand : Land
{
    [SerializeField]
    private NavMeshSurface surface;
    public NavMeshSurface GetNavMeshSurface => surface;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
