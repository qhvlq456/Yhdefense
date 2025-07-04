using UnityEngine;

public class EnemyAnimation : CharacterAnimation
{
    [SerializeField]
    private Enemy enemy;
    // Animation Event에서 호출
    public void OnHitAnimationEndEvent()
    {
        // Debug.LogError("Hit Enemy Animation Ended");
        ChangeState(AnimationState.Walk);
    }
    public void OnDeathAnimationEndEvent()
    {
        // Debug.LogError("Hit Enemy Animation Death Ended");
        enemy.OnDeath();
    }
}
