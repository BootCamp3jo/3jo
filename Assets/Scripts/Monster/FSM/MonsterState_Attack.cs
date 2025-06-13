// 공격 상태의 기본형. 해당 클래스를 상속받아 패턴 구현
using UnityEngine;
public class MonsterState_Attack : IState
{
    public MonsterStateMachine stateMachine;
    public MonsterState_Attack(MonsterStateMachine monsterStateMachine)
    {
        this.stateMachine = monsterStateMachine;
    }

    public virtual void Enter() 
    {
        // 어떤 패턴으로 진입할지 설정 + 공격 모션 + 공격 패턴 호출
        stateMachine.monster.ChoiceAttack();
    }
    public virtual void Execute() 
    {
        // 공격 모션이 끝났다면 idle로 돌아가기
       if(!stateMachine.monster.isAttacking)
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    }

    public virtual void Exit(){}
}
