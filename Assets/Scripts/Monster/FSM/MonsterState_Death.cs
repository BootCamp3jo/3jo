
public class MonsterState_Death : IState
{
    MonsterStateMachine StateMachine;
    public MonsterState_Death(MonsterStateMachine monsterStateMachine)
    {
        StateMachine = monsterStateMachine;
    }

    public void Enter()
    {
        // 죽음 애니메이션 1번만 실행 >> 죽음 애니메이션.. 없다?
        StateMachine.SetAnimeTrigger(StateMachine.AnimatorParameters.deathHash);
    }

    public void Execute(){}

    public void ExecutePhysically() { }

    public void Exit(){}
}
