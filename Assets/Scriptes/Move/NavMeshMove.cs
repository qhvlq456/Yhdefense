using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshMove : Move
{
    [SerializeField] private NavMeshAgent agent;

    [Header("Agent Settings")]
    [SerializeField] private float angularSpeed = 1000;
    [SerializeField] private float acceleration = 24f;
    [SerializeField] private int avoidancePriority = 50;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // enemydata가 아닌 movedata로 따로 빼놓을 것!
    private void SetupAgent(MoveData _moveData)
    {
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.autoRepath = true;
        agent.avoidancePriority = avoidancePriority;
        agent.updatePosition = true;
        agent.updateRotation = false;
        agent.stoppingDistance = 0.1f; // 너무 멀리서 멈추는 거 방지
    }

    public override void Initialize(MoveData _moveData)
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

        SetupAgent(_moveData);
    }

    private void LateUpdate()
    {
        if (agent.enabled && agent.hasPath && agent.velocity.sqrMagnitude > 0.01f)
        {
            Vector3 direction = agent.velocity.normalized;
            direction.y = 0;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
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
