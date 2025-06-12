
public class MonsterState_Death : IState
{
    MonsterStateMachine StateMachine;
    public MonsterState_Death(MonsterStateMachine monsterStateMachine) => StateMachine = monsterStateMachine;

    public void Enter()
    {
        // 
        StateMachine.StartAnimeTrigger(StateMachine.AnimatorParameters.deathHash);
    }

    public void Execute(){}

    public void Exit(){}
}
