using UnityEngine;

public class FlyMove : Move
{
    [SerializeField]
    private Vector3 destination = Vector3.zero;

    public override void Initialize(MoveData _moveData)
    {
        moveData = _moveData;
    }
    private void Update()
    {
        // 목적지에 도착하지 않았으면 이동
        if (!IsArrived())
        {
            float step =  moveData.moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
            //Vector3 direction = (destination - transform.position).normalized;
            //Quaternion rot = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * moveData.rotationSpeed);
        }
    }

    public override void Movement(Vector3 _destination)
    {
        destination = _destination;

        Vector3 direction = (_destination - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(direction);
        transform.rotation = rot;
    }

    public override void Revert()
    {
        onAnimationChange = null;
        Stop();
    }

    public override void Stop()
    {
        // 필요시 이동 중단 로직 구현
    }

    public override bool IsArrived(float threshold = 0.2f)
    {
        bool ret = Vector3.Distance(transform.position, destination) <= threshold;
        // 도착
        if (ret)
        {
            transform.position = destination; // 정확한 위치로 이동
            onAnimationChange?.Invoke(CharacterAnimation.AnimationState.Fall);
        }

        return ret;
    }
}
