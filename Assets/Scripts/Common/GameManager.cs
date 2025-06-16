using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    // 보스는 하나만 존재하기에 매니저에서 접근할 수 있게 하면 좋지 않을까?
    public MonsterBase boss;

    public void InitBoss(MonsterBase boss)
    {
        this.boss = boss;
    }
}
