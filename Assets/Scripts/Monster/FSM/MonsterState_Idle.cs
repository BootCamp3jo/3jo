using UnityEngine;

public class MonsterState_Idle : IState
{
    MonsterStateMachine stateMachine;
    public MonsterState_Idle(MonsterStateMachine monsterStateMachine)
    {
        this.stateMachine = monsterStateMachine;
    }

    public void Enter()
    {
        // 대기 모션 재생 시작
        stateMachine.StartAnime(stateMachine.AnimatorParameters.idleHash);
    }

    // 대기하는 몬스터
    public virtual void Execute()
    {
        // 몬스터 공격 딜레이를 줄여주다가 끝났다면, 공격 상태로!
        stateMachine.monster.Idle();
    }

    // 거리 체크
    public void ExecutePhysically()
    {
    }

    public void Exit()
    {
        // 대기 모션 재생 종료
        stateMachine.StopAnime(stateMachine.AnimatorParameters.idleHash);
    }
}
