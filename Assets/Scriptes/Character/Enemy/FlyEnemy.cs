using UnityEngine;
using System.Collections;


public class FlyEnemy : Enemy
{
    [SerializeField]
    private float waitForFlyMove = 0.15f; // 날아다니기 전 대기 시간

    [SerializeField]
    protected float flyHeight = 1f; // 적이 날아다닐 높이
    public override void Set(int _idx)
    {
        base.Set(_idx);
        enemyAnimation.ChangeState(CharacterAnimation.AnimationState.Walk);
        move.onAnimationChange += enemyAnimation.ChangeState; // 이동 애니메이션 변경 이벤트 등록
        StartCoroutine(CoFlyMove());
    }
    public override void Spawn(Vector3 _spawnPos, Vector3 _destination)
    {
        transform.position = _spawnPos + Vector3.up * flyHeight;
        _destination += Vector3.up * flyHeight; // 목적지 위치를 날아다닐 높이만큼 조정
        move.Movement(_destination);
        base.Spawn(_spawnPos, _destination);
    }

    IEnumerator CoFlyMove()
    {
        yield return new WaitForSeconds(waitForFlyMove);
        enemyAnimation.ChangeState(CharacterAnimation.AnimationState.Fly);
    }
}
