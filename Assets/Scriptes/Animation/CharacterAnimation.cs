using UnityEngine;

[RequireComponent(typeof(Animator))] 
public class CharacterAnimation : MonoBehaviour
{
    public enum AnimationState
    {
        Idle,
        Walk,
        Run,
        Jump,
        Fall,
        Attack,
        Hit,
        Death
    }

    [SerializeField]
    protected Animator animator;

    [SerializeField]
    protected AnimationState currentState = AnimationState.Idle;
    public virtual void ChangeState(AnimationState _animationState)
    {
        if (currentState == _animationState)
        {
            return;
        }

        currentState = _animationState;

        switch (currentState)
        {
            case AnimationState.Idle:
                animator.SetBool("IsWalk", false);
                break;
            case AnimationState.Walk:
                animator.SetBool("IsWalk", true);
                break;
            case AnimationState.Hit:
                animator.SetTrigger("Hit");
                animator.SetBool("IsWalk", false);
                break;
            case AnimationState.Death:
                animator.SetTrigger("Death");
                animator.SetBool("IsWalk", false);
                break;
        }
    }
}
