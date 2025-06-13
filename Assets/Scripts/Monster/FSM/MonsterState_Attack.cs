// 공격 상태의 기본형. 해당 클래스를 상속받아 패턴 구현
public class MonsterState_Attack : IState
{
    protected MonsterStateMachine stateMachine;
    protected MonsterState_Attack(MonsterStateMachine monsterStateMachine)
    {
        this.stateMachine = monsterStateMachine;
    }

    public virtual void Enter() 
    {


        // 공격 상태 진입
        stateMachine.SetAnimeTrigger(stateMachine.AnimatorParameters.attackLayerHash);
    }

    public virtual void Execute() {}

    public virtual void Exit(){}
}
