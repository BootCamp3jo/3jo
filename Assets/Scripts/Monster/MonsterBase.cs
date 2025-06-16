using Unity.Mathematics;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class MonsterBase : ANPC
{
    [field: SerializeField] public MonsterData monsterData;
    Pattern[] patterns;

    // 타겟이 될 대상의 레이어(보통은 플레이어).. 그런데 레이어가 필요한가? 플레이어 트랜스폼만 있으면 되는 게 아닐까?
    // 게임매니저에 플레이어에 접근할 수 있도록 했다고 하니 그걸 써보자
    // 패턴을 만들다 보면 타겟이 플레이어가 아니게 되는 경우도 있으니 이름을 타겟으로 변경
    Transform target;

    SpriteRenderer spriteRenderer;
    public Animator animator { get; private set; }

    // 상태 머신 관리용 클래스
    public MonsterStateMachine stateMachine { get; private set; }

    float atk;
    // 현재 쿨타임(보스 패턴마다 쿨타임이 달라 기술 사용과 동시에 써주기!!!)
    // 플레이어가 반응할 시간을 주기 위해 초기값
    public float atkDelay { get; private set; } = 1f;

    // 쥬금 상태
    bool isDead;

    // 움직일 때 true면 도망, false면 추격
    bool isFlee;

    // 이번에 공격할 패턴의 인덱스
    int atkIndex = 0;
    // 공격 패턴 재생 중인지 여부
    public bool isAttacking { get; private set; } = false;

    // 패턴들의 최대/최소 거리의 배열!
    float2[] distanceRangePatterns;
    // 이번에 공격 가능한 패턴들(공격은 자주 이뤄지기에 매번 생성하기보다 하나를 생성하고 Clear()하면 더 낫지 않을까?)
    List<int> patternsAvailable = new List<int>();

    // 이동 벡터
    Vector2 moveVec;

    // 패턴을 발동할 수 있는 거리의 제곱의 영역 x~y 사이. x보다 작거나 y보다 크면 각각 후퇴,추적
    float2 distPoweredBoundary = new(float.MaxValue, float.MinValue);


    // 적 이펙트 관련
    [Header("피격 관련(깜빡임)")]
    [SerializeField] private FlashEffect flashEffect;

    [Header("피격 관련(파티클)")]
    [SerializeField] private HitEffect hitEffect;

    [Header("피격 관련(진동)")]
    [SerializeField] private ShakeEffect shakeEffect;

    // 적 체력 관련
    private float maxHp;
    public event Action<float> onHpChanged;
    [SerializeField] private EnemyHpBar enemyHpBar;

    // 경험치 관련 << 추후 Data 에 넣어줘야함
    public int exp = 50;

    protected override void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        flashEffect = GetComponent<FlashEffect>();
        hitEffect = GetComponent<HitEffect>();
        shakeEffect = GetComponent<ShakeEffect>();
        stateMachine = new MonsterStateMachine(this);

        // 체력바 생성
        if (enemyHpBar != null)
        {
            enemyHpBar = Instantiate(enemyHpBar);
            onHpChanged += enemyHpBar.SetHp;
        }
        maxHp = npcData.hp;

        base.Awake();
    }

    private void OnEnable()
    {
        // 패턴들을 모두 받아오기
        List<Pattern> patternList = new List<Pattern>();
        for (int i = 0; i < ObjectPoolManager.Instance.prefabs.Length; i++)
        {
            patternList.Add(ObjectPoolManager.Instance.prefabs[i].GetComponent<Pattern>());
        }
        patterns = patternList.ToArray();
        // 실행 때 비활성화 상태의 패턴이 있다면 활성화
        // Awake()에서 필요한 컴포넌트를 받아오게 한 뒤 패턴 비활성화(호출에 대한 준비 완료)
        for (int i = 0; i < patterns.Length; i++)
        {
            if (!patterns[i].gameObject.activeSelf)
                patterns[i].gameObject.SetActive(true);
        }
    }

    protected override void Start()
    {
        // 게임매니저의 보스에 자신을 등록
        GameManager.Instance.InitBoss(this);

        // 시작할 때 현재 HP를 최대 HP로 >> ANPC에서 할당

        atk = monsterData.atk;

        // 플레이어를 타겟으로
        target = PlayerManager.Instance.playerPrefab.transform;

        // 거리 비교할 때 Vector2.SqrMagnitude 를 사용할 것이기에 미리 제곱한 값을 가지고 있도록
        distanceRangePatterns = new float2[patterns.Length];
        for (int i = 0; i < distanceRangePatterns.Length; i++)
        {
            float2 tmpRange = patterns[i].patternData.range;
            // 제곱값
            float2 tmpRangePow = new float2(tmpRange.x * tmpRange.x, tmpRange.y * tmpRange.y);
            distanceRangePatterns[i] = tmpRangePow;

            // 패턴 중 가장 작은 발동 가능 범위 값의 제곱
            if (distPoweredBoundary.x > tmpRangePow.x)
                distPoweredBoundary.x = tmpRangePow.x;
            // 패턴 중 가장 큰 발동 가능 범위 값의 제곱
            if(distPoweredBoundary.y < tmpRangePow.y)
                distPoweredBoundary.y = tmpRangePow.y;
        }

        base.Start();
    }

    private void Update()
    {
        // 죽었다면 다른 동작을 하지 않도록
        if (isDead) return;
        // 액션에 맞게 타겟 방향/반대 방향을 바라보도록
        ChangeFlipX();

        // 요기서 현 상태의 Execute 실행!
        stateMachine.Execute();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // idle 상태일 때 딜레이 감소 및 공격 상태 전환
    public void Idle()
    {
        if (!isAttacking)
        {
            // 공격 상태로 전환
            stateMachine.ChangeState(stateMachine.attackState);
            return;
        }
    }

    // 보스의 이동 관련
    public void Move()
    {
        // 추적 속도 벡터
        moveVec = (PlayerManager.Instance.player.transform.position- transform.position).normalized * monsterData.moveSpeed * Time.deltaTime;
        // 도망은 추적의 역방향으로 가게끔
        if (isFlee)
            moveVec = -moveVec;

        transform.position += (Vector3)moveVec;
    }

    public void StartFlee()
    {
        isFlee = true;
    }

    public void FaceToTarget()
    {
        isFlee = false;
    }

    // 액션에 따라 자연스럽게 보이게 x플립
    void ChangeFlipX()
    {
        spriteRenderer.flipX = isFlee ? (target.transform.position.x > transform.position.x) : (target.transform.position.x < transform.position.x );
    }

    // 대미지 계산
    public void GetDamage(float damage)
    {
        if (isDead) return;

        hitEffect.PlayHitEffect(1);
        flashEffect.TriggerFlash();
        shakeEffect.Shake();

        npcData.hp = Mathf.Max(npcData.hp-damage, 0);
        onHpChanged?.Invoke(npcData.hp/ maxHp);
        if (npcData.hp <= 0)
            Dead();
    }

    // 사망 처리
    protected void Dead()
    {
        // 사망 상태로 전환
        isDead = true;
        // 죽음 모션
        stateMachine.ChangeState(stateMachine.deathState);
        // 아이템 드랍 !!!
        ExpManager.instance.SpawnExp(transform.position, exp);

        // 다음 스테이지로의 문이 열림 !!!
    }

    // 어떤 공격 패턴을 사용할지 정하고, 이에 맞는 애니메이션 전환
    // distPowered = 타겟과의 거리의 제곱
    public void ChoiceAttack()
    {
        // 공격 중일 때 다시 들어오지 않도록
        if(isAttacking) return;
        // 공격 애니메이션 재생 중으로 변화
        isAttacking = true;
        // 재생할 공격 애니메이션
        int tmpAtkIndex = 0;
        // 패턴 값들 초기화
        patternsAvailable.Clear();
        float distPowered = Vector2.SqrMagnitude(target.transform.position - transform.position);
        // 미리 제곱으로 기억해둔 값과 비교하여 어떤 공격들이 적합한지 판정
        for (int i = 0; i < distanceRangePatterns.Length; i++)
        {
            // 해당 패턴의 사거리 범위 안에 있다면, 사용 가능한 패턴 리스트에 추가
           if(distanceRangePatterns[i].x < distPowered && distPowered < distanceRangePatterns[i].y)
            {
                patternsAvailable.Add(i);
            }
        }
        // 적합한 공격들이 있다면, 해당 공격들 중 랜덤
        if (patternsAvailable.Count > 0)
        {
            tmpAtkIndex = patternsAvailable[UnityEngine.Random.Range(0, patternsAvailable.Count)];
            // 어느 공격으로 전환할지 int 설정
            animator.SetInteger(stateMachine.AnimatorParameters.attackHash, tmpAtkIndex);
        }
        atkIndex = tmpAtkIndex;

        // 패턴에 랜덤성이 있다면, 애니메이터에 해당 랜덤값을 부여 및 이에 따른 값 세팅
        IRandomAtkCount randomAtkCountable;
        randomAtkCountable = patterns[tmpAtkIndex].patternData as IRandomAtkCount;
        if (randomAtkCountable != null)
        {
            // 범위 내 랜덤한 공격 수
            int randomAtkCountTmp = UnityEngine.Random.Range(randomAtkCountable.randomAtkCount.x, randomAtkCountable.randomAtkCount.y);
            // 애니메이터에 공격 수 넣기
            animator.SetInteger(stateMachine.AnimatorParameters.attackCountRemainHash, randomAtkCountTmp);
        }
        else
        {
            // 랜덤 공격 횟수를 지정하지 않는 스킬은 1번만
            animator.SetInteger(stateMachine.AnimatorParameters.attackCountRemainHash, 1);
            atkDelay = patterns[atkIndex].patternData.delay;
        }
        // 공격 레이어 진입 >> 설정했던 공격 index로 진입
        // 설정에 실패하였다면 0번째 공격으로 자동 진입 >> 가장 넓은 사거리 영역에서 보편적으로 쓰일 수 있는 패턴을 0번째에 두면 베스트!
        animator.SetTrigger(stateMachine.AnimatorParameters.attackLayerHash);

        // 사용한 기술에 맞는 쿨타임 재설정
        atkDelay = patterns[atkIndex].patternData.delay;
    }

    // 공격 이후 딜레이 동안 잠깐 무방비(공격하기 좋은 시간) >> 이후 다음 공격
    public void StartDelay()
    {
        Invoke("AfterDelay", atkDelay);
    }

    void AfterDelay()
    {
        isAttacking = false;
        stateMachine.ChangeState(stateMachine.idleState);
    }

    #region Animation Event Methods
    // 공격 애니메이션 도중 패턴 생성 타이밍에 맞춰 생성
    public void CallPattern()
    {
        // 한번에 공격하는 수
        int atkInOneTime = patterns[atkIndex].patternData.atkNumForOnce;
        // 남은 공격 수에서 그만큼 실행
        for (int i = 0; i < atkInOneTime; i++)
        {
            // 애니메이터에서 공격 횟수가 남았다면, 카운터를 하나 줄이고 공격 1회 수행
            int atkRemainCount = animator.GetInteger(stateMachine.AnimatorParameters.attackCountRemainHash);
            if (atkRemainCount == 0)
                break;
            animator.SetInteger(stateMachine.AnimatorParameters.attackCountRemainHash, atkRemainCount - 1);
            // 오브젝트 풀에서 패턴 가져오기
            ObjectPoolManager.Instance.GetObject(atkIndex, Vector2.zero, quaternion.identity);
        }
    }
    #endregion
}
