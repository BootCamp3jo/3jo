using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    // 보스는 하나만 존재하기에 매니저에서 접근할 수 있게 하면 좋지 않을까?
    public MonsterBase boss;
    // 슬로우 존 내부의 슬로우 가능한 오브젝트들(적, 패턴)의 속도감소율
    [field:SerializeField] public float slowRate { get; private set; }

    public void InitBoss(MonsterBase boss)
    {
        this.boss = boss;
    }
}
