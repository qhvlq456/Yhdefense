using UnityEngine;

public abstract class Move : MonoBehaviour
{
    public MoveData moveData { get; private set; }
    public abstract void Initialize(MoveData _moveData);
    public abstract void Movement(Vector3 _destination);
    public abstract void Stop();
    public abstract void Revert();
}
