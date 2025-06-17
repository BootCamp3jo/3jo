using System;

public class MonsterState_Move : IState
{
    MonsterStateMachine stateMachine;
    public MonsterState_Move(MonsterStateMachine monsterStateMachine)
    {
        this.stateMachine = monsterStateMachine;
    }

    public void Enter()
    {
        // 이동 모션 재생 시작(이동 스프라이트가 없다면 이후 잔상 효과로 대체 !!!)
        stateMachine.StartAnime(stateMachine.AnimatorParameters.moveHash);
    }

    public void Execute(){}

    public void ExecutePhysically()
    {
        stateMachine.monster.Move();
    }

    public void Exit()
    {
        // 대기 모션 재생 종료
        stateMachine.StopAnime(stateMachine.AnimatorParameters.moveHash);
    }
}
