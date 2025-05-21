using UnityEngine;

public abstract class Move : MonoBehaviour
{
    public abstract void Initialize(MoveData _moveData);
    public abstract void Movement(Vector3 _destination);
    public abstract void Stop();
    public abstract void Revert();
}
