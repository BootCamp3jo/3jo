using UnityEngine;

public abstract class MonsterBase : ANPC
{
    [field: SerializeField] public MonsterData monsterData;

    // 타겟이 될 플레이어의 레이어
    [field: SerializeField] public LayerMask player { get; private set; }

    public MonsterStateMachine monsterStateMachine { get; private set; }

    // 현재 체력
    float hpCurrent;
    // 현재 쿨타임(보스 패턴마다 쿨타임이 달라 기술 사용과 동시에 써주기!!!)
    public float atkDelay { get; set; }

    protected override void Awake()
    {
        base.Awake();
        //monsterStateMachine = new MonsterStateMachine(this);
    }

    protected override void Start()
    {
        base.Start();
        // 시작할 때 현재 HP를 최대 HP로
        hpCurrent = monsterData.hp;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

    }

    public void ChangeAtkDelay(float newDelay)
    {
        atkDelay = newDelay;
    }

    // 대미지 계산
    public void GetDamage(float damage)
    {
        if (hpCurrent <= 0) return;
        hpCurrent = Mathf.Max(hpCurrent-damage, 0);
        // 피격 이펙트가 있다면 여기에!
        if (hpCurrent <= 0)
            Dead();
    }

    // 사망 처리
    protected void Dead()
    {
        // 죽음 모션
        monsterStateMachine.ChangeState(monsterStateMachine.deathState);
    }
}
