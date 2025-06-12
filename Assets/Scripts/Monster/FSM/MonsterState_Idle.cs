using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_Idle : IState
{
    MonsterStateMachine monsterStateMachine;
    public MonsterState_Idle(MonsterStateMachine monsterStateMachine) => this.monsterStateMachine = monsterStateMachine;

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
