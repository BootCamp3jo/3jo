using Unity.Mathematics;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterBase : ANPC
{
    [field: SerializeField] public MonsterData monsterData;

    // 패턴들을 넣어둔 자식 오브젝트
    [SerializeField] Transform patternsParent;
    Pattern[] patterns;

    // 타겟이 될 대상의 레이어(보통은 플레이어).. 그런데 레이어가 필요한가? 플레이어 트랜스폼만 있으면 되는 게 아닐까?
    // 게임매니저에 플레이어에 접근할 수 있도록 했다고 하니 그걸 써보자
    // 패턴을 만들다 보면 타겟이 플레이어가 아니게 되는 경우도 있으니 이름을 타겟으로 변경
    Transform target;

    public Animator animator { get; private set; }

    // 상태 머신 관리용 클래스
    public MonsterStateMachine stateMachine { get; private set; }

    // 현재 체력
    float hpCurrent;
    float atk;
    // 현재 쿨타임(보스 패턴마다 쿨타임이 달라 기술 사용과 동시에 써주기!!!)
    // 플레이어가 반응할 시간을 주기 위해 초기값 1초
    public float atkDelay { get; private set; } = 1f;

    // 쥬금 상태
    bool isDead;

    // 이번에 공격할 패턴의 인덱스
    int atkIndex = 0;
    // 공격 패턴 재생 중인지 여부
    public bool isAttacking { get; private set; } = false;

    // 패턴들의 최대/최소 거리의 배열!
    float2[] distanceRangePatterns;
    // 이번에 공격 가능한 패턴들(공격은 자주 이뤄지기에 매번 생성하기보다 하나를 생성하고 Clear()하면 더 낫지 않을까?)
    List<int> patternsAvailable = new List<int>();

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        stateMachine = new MonsterStateMachine(this);

        base.Awake();
    }

    protected override void Start()
    {
        // 시작할 때 현재 HP를 최대 HP로
        hpCurrent = monsterData.hpMax;
        atk = monsterData.atk;

        patterns = patternsParent.GetComponentsInChildren<Pattern>(true);

        // 플레이어를 타겟으로
        target = PlayerManager.Instance.playerPrefab.transform;

        distanceRangePatterns = new float2[monsterData.patternDatas.Length];
        for (int i = 0; i < distanceRangePatterns.Length; i++)
        {
            float2 tmpRange = monsterData.patternDatas[i].range;
            // 거리 비교할 때 Vector2.SqrMagnitude 를 사용할 것이기에 미리 제곱한 값을 가지고 있도록
            distanceRangePatterns[i] = new float2(tmpRange.x * tmpRange.x, tmpRange.y * tmpRange.y);
        }

        // 에러 때문에 일시적으로 아래로 뺌
        base.Start();
    }

    private void Update()
    {
        // 죽었다면 다른 동작을 하지 않도록
        if (isDead) return;
        // 요기서 현 상태의 Execute 실행!
        stateMachine.Execute();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    // idle 상태일 때 딜레이 감소 및 공격 상태 전환에 호출
    public void DecreaseDelay(float Amount)
    {
        // 공격 딜레이 감소
        atkDelay -= Amount;
        if (atkDelay <= 0)
        {
            // 공격 애니메이션 재생 중으로 변화
            isAttacking = true;
            // 공격 상태로 전환
            stateMachine.ChangeState(stateMachine.attackState);
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
        stateMachine.ChangeState(stateMachine.deathState);
        // 아이템 드랍 !!!
        // 다음 스테이지로의 문이 열림 !!!
    }

    // 어떤 공격 패턴을 사용할지 정하고, 이에 맞는 애니메이션 전환
    public void ChoiceAttack()
    {
        // 재생할 공격 애니메이션
        int tmpAtkIndex = 0;
        // 패턴 값들 초기화
        patternsAvailable.Clear();
        // 거리의 제곱값
        float distBetTarget = Vector2.SqrMagnitude(target.transform.position-transform.position);
        // 미리 제곱으로 기억해둔 값과 비교하여 어떤 공격들이 적합한지 판정
        for (int i = 0; i < distanceRangePatterns.Length; i++)
        {
            // 해당 패턴의 사거리 범위 안에 있다면, 사용 가능한 패턴 리스트에 추가
           if(distanceRangePatterns[i].x < distBetTarget && distBetTarget < distanceRangePatterns[i].y)
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

        // 공격 레이어 진입 >> 설정했던 공격 index로 진입
        // 설정에 실패하였다면 0번째 공격으로 자동 진입 >> 가장 넓은 사거리 영역에서 보편적으로 쓰일 수 있는 패턴을 0번째에 두면 베스트!
        animator.SetTrigger(stateMachine.AnimatorParameters.attackLayerHash);
        atkIndex = tmpAtkIndex;

        // 사용한 기술에 맞는 쿨타임 재설정
        atkDelay = monsterData.patternDatas[atkIndex].delay;
    }

    #region Animation Event Methods
    // 공격 애니메이션 도중 패턴 생성 타이밍에 맞춰 생성
    public void CallPattern()
    {
        // 패턴들에 필요한 것을 넘겨주기
        // 몬스터 위치, 대미지
        patterns[atkIndex].GetAtkData(transform.position, atk * monsterData.patternDatas[atkIndex].atkCoefficient);
        // 발동중인 모션에 맞는 것을 활성화
        patternsParent.GetChild(atkIndex).gameObject.SetActive(true);
    }

    // 공격 애니메이션 끝날 때 상태 변경용으로 알려주기
    public void EndAttack()
    {
        isAttacking = false;
    }
    #endregion
}
