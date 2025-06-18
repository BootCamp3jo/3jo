using UnityEngine;

public class Monster_MartialHero : MonsterBase
{
    public Collider2D[] MeleeTriggers;
    int enabledTriggerIndex = 0;

    Vector3 flipScale = new Vector3(-1, 1, 1);

    // 애니메이션 이벤트에는 enum 매개변수를 받을 수 없기에 int로 치환하여 적용
    int[] soundSfxInt = new int[3] { (int)SFXType.MartialHero_Attack, (int)SFXType.MartialHero_SPAttack,(int)SFXType.MartialHero_Sheath };

    // idle 상태일 때 딜레이 감소 및 공격 상태 전환
    public override void Idle()
    {
        // Idle 상태에서 Update마다 체크
        Vector2 posDiff = PlayerManager.Instance.playerPrefab.transform.position - transform.position;
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
        Vector2 posDiff = PlayerManager.Instance.playerPrefab.transform.position - transform.position;
        // 속도만큼 이동
        moveVec = posDiff.normalized * monsterData.moveSpeed * Time.deltaTime;
        transform.position += (Vector3)moveVec;

        // 공격 가능한 거리 안으로 들어갔다면, 공격 시작
        distPowered = Vector2.SqrMagnitude(posDiff);
        if (distPowered < distPoweredBoundary.y && !isAttacking)
            stateMachine.ChangeState(stateMachine.attackState);
    }

    // 마셜 히어로는 공격 모션 도중에 SFX를 집어넣어 이를 패턴 전조로 활용하기에 발동 시점을 이동
    public override void ChoiceAttack()
    {
        ChoiceAtk_NoSound();
    }

    public void AtkSoundPlay(int soundIndex)
    {
        AudioManager.instance.PlaySFX((SFXType)soundSfxInt[soundIndex]);
    }

    // 보스가 근접 공격을 할 때 어떤 트리거를 활성화할지
    public virtual void EnableMeleeTrigger(int index)
    {
        // 기존의 공격 트리거를 비활성화
        MeleeTriggers[enabledTriggerIndex].enabled = false;

        // 다음 공격 트리거 활성화
        enabledTriggerIndex = index;
        // 좌우 플립에 따라서 영역도 뒤집어주기
        if (spriteRenderer.flipX)
            MeleeTriggers[enabledTriggerIndex].transform.localScale = flipScale;
        else
            MeleeTriggers[enabledTriggerIndex].transform.localScale = Vector3.one;
        MeleeTriggers[enabledTriggerIndex].enabled = true;
    }
    // 공격이 끝날 때 해당 트리거 비활성화
    public virtual void DisableMeleeTrigger()
    {
        MeleeTriggers[enabledTriggerIndex].enabled = false;
    }
}
