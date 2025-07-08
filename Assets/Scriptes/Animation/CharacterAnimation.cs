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
        Fly,
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
                animator.SetBool("IsIdle", true);
                animator.SetBool("IsWalk", false);
                break;
            case AnimationState.Walk:
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsWalk", true);
                break;
            case AnimationState.Fly:
                animator.SetBool("IsWalk", false);
                animator.SetBool("IsFly", true);
                break;
            case AnimationState.Fall:
                animator.SetBool("IsFly", false);
                animator.SetBool("IsFall", true);
                break;
            case AnimationState.Hit:
                animator.SetTrigger("Hit");
                //animator.SetBool("IsIdle", false);
                //animator.SetBool("IsWalk", false);
                break;
            case AnimationState.Death:
                animator.SetTrigger("Death");
                //animator.SetBool("IsIdle", false);
                //animator.SetBool("IsWalk", false);
                break;
        }
    }
}
