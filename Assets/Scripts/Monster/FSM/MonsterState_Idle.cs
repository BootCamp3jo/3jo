using System.Collections;
using System.Collections.Generic;
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

    public void Execute()
    {
        //stateMachine.monster.
    }

    public void Exit()
    {
        // 대기 모션 재생 종료
        stateMachine.StopAnime(stateMachine.AnimatorParameters.idleHash);
    }
}
