using UnityEngine;
using UnityEngine.AI;

public class NavMeshMove : Move
{
    [SerializeField]
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public override void Initialize()
    {
        agent.enabled = true;
        agent.isStopped = false;
    }

    public override void Movement(Vector3 _destination)
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("Agent not on NavMesh");
            return;
        }
        agent.SetDestination(_destination);
    }

    public override void Stop()
    {
        if (agent.isOnNavMesh)
            agent.isStopped = true;
    }

    public override void Revert()
    {
        Stop();
        agent.enabled = false;
    }
}
