using UnityEngine;

public class Monster_MartialHero : MonsterBase
{
    // idle 상태일 때 딜레이 감소 및 공격 상태 전환
    public override void Idle()
    {
        // Idle 상태에서 Update마다 체크
        Vector2 posDiff = target.position - transform.position;
        distPowered = Vector2.SqrMagnitude(posDiff);

        // 최대 사거리를 벗어났다면, 이동 상태로
        if (distPowered > distPoweredBoundary.y)
            stateMachine.ChangeState(stateMachine.moveState);
        // 공격 최대 사거리 안이고 공격 가능한 상태라면
        else if (!isAttacking)
        {
            // 공격 상태로 전환
            stateMachine.ChangeState(stateMachine.attackState);
            return;
        }
    }

    public override void Move()
    {
        // 추적 속도 벡터
        Vector2 posDiff = target.position - transform.position;
        // 속도만큼 이동
        moveVec = posDiff.normalized * monsterData.moveSpeed * Time.fixedTime;
        transform.position += (Vector3)moveVec;

        // 공격 가능한 거리 안으로 들어갔다면, 공격 시작
        distPowered = Vector2.SqrMagnitude(posDiff);
        if (distPowered < distPoweredBoundary.y && !isAttacking)
            stateMachine.ChangeState(stateMachine.attackState);
    }
}
