using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMove : Move
{
    [SerializeField] private NavMeshAgent agent;

    [Header("Agent Settings")]
    [SerializeField] private float angularSpeed = 3;
    [SerializeField] private float acceleration = 24f;
    [SerializeField] private int avoidancePriority = 50;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SetupAgent(); // 설정 통합
    }

    private void SetupAgent()
    {
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.autoRepath = true;
        agent.avoidancePriority = avoidancePriority;
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.stoppingDistance = 0.1f; // 너무 멀리서 멈추는 거 방지
    }

    public override void Initialize()
    {
        if (!agent.enabled)
        {
            agent.enabled = true;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("Agent is not on the NavMesh at Initialize.");
            return;
        }

        agent.isStopped = false;
        SetupAgent(); // 초기화 시 재세팅 보장
    }

    public override void Movement(Vector3 _destination)
    {
        if (!agent.isOnNavMesh)
        {
            Debug.LogWarning("Agent not on NavMesh, cannot move.");
            return;
        }

        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        agent.SetDestination(_destination);
    }

    public override void Stop()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
    }

    public override void Revert()
    {
        Stop();

        if (agent.enabled)
        {
            agent.enabled = false;
        }
    }

    public bool IsArrived(float threshold = 0.2f)
    {
        // 목적지 도착 체크 유틸
        return !agent.pathPending &&
               agent.remainingDistance <= threshold &&
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
}
