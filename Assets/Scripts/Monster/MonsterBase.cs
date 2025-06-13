using UnityEngine;

public abstract class MonsterBase : ANPC
{
    [field: SerializeField] public MonsterData monsterData;

    // 타겟이 될 대상의 레이어(보통은 플레이어).. 그런데 레이어가 필요한가? 플레이어 트랜스폼만 있으면 되는 게 아닐까?
    // 게임매니저에 플레이어에 접근할 수 있도록 했다고 하니 그걸 써보자
    // 패턴을 만들다 보면 타겟이 플레이어가 아니게 되는 경우도 있으니 이름을 타겟으로 변경
    Transform target;

    public Animator animator { get; private set; }

    // 상태 머신 관리용 클래스
    public MonsterStateMachine monsterStateMachine { get; private set; }

    // 현재 체력
    float hpCurrent;
    // 현재 쿨타임(보스 패턴마다 쿨타임이 달라 기술 사용과 동시에 써주기!!!)
    // 플레이어가 반응할 시간을 주기 위해 초기값 1초
    public float atkDelay { get; private set; } = 1f;

    // 쥬금 상태
    bool isDead;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        monsterStateMachine = new MonsterStateMachine(this);
    }

    protected override void Start()
    {
        base.Start();
        // 시작할 때 현재 HP를 최대 HP로
        hpCurrent = monsterData.hpMax;
    }

    private void Update()
    {
        // 죽었다면 다른 동작을 하지 않도록
        if (isDead) return;
        // 요기서 현 상태의 Execute 실행!
        monsterStateMachine.Execute();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // 패턴 사용 후 해당 패턴의 딜레이로 교체할 때 호출
    public void ChangeAtkDelay(float newDelay)
    {
        atkDelay = newDelay;
    }

    // idle 상태일 때 딜레이 감소 및 공격 상태 전환에 호출
    public void DecreaseDelay(float Amount)
    {
        // 공격 딜레이 감소
        atkDelay -= Amount;
        if (atkDelay <= 0)
        {
            // 공격 상태로 전환
            monsterStateMachine.ChangeState(monsterStateMachine.attackState);
        }
    }

    // 대미지 계산
    public void GetDamage(float damage)
    {
        if (isDead) return;
        // 피격 이펙트가 있다면 여기에!
        hpCurrent = Mathf.Max(hpCurrent-damage, 0);
        if (hpCurrent <= 0)
            Dead();
    }

    // 사망 처리
    protected void Dead()
    {
        // 사망 상태로 전환
        isDead = true;
        // 죽음 모션
        monsterStateMachine.ChangeState(monsterStateMachine.deathState);
        // 아이템 드랍 !!!
        // 다음 스테이지로의 문이 열림 !!!
    }

    public void DetectTarget()
    {

        target
    }
}
