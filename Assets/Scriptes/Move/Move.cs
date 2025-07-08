using UnityEngine;

public abstract class Move : MonoBehaviour
{
    public MoveData moveData { protected get; set; }
    public abstract void Initialize(MoveData _moveData);
    public abstract void Movement(Vector3 _destination);
    public virtual bool IsArrived(float threshold = 0.2f) { return default; }
    public System.Action<CharacterAnimation.AnimationState> onAnimationChange;
    public abstract void Stop();
    public abstract void Revert();
}
