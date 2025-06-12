using UnityEngine;

// 몬스터 FSM 각 상태 클래스에서 공통으로 구현할 구조를 미리 정의
public interface IState
{
    void Enter(); // 해당 상태에 진입할 때 수행할 내용
    void Execute(); // 해당 상태 도중 수행할 내용
    void Exit(); // 해당 상태에서 나갈 때 수행할 내용
}

public class MonsterStateMachine : MonoBehaviour
{
    // 상태 중에 몬스터의 정보를 참 있기에 연결!
    public MonsterBase monster {get; private set;}
    public MonsterStateMachine(MonsterBase monster) => this.monster = monster;

    // 현재 어떤 상태인지 넣어둘 변수
    private IState currentState;

    // 애니메이터와 파라미터
    Animator animator;
    public MonsterAnimatorParameters AnimatorParameters = new();

    // 각 상태를 정의해주기(필요한 상태 추가)
    public MonsterState_Idle idleState { get; private set; }
    public MonsterState_Death deathState { get; private set; }

    void Start()
    {
        animator = GetComponent<Animator>();

        // 각 상태 초기화
        idleState = new MonsterState_Idle(this);
        deathState = new MonsterState_Death(this);

        // 처음은 대기
        ChangeState(idleState);
    }

    // Execute()는 이곳에서 계속 수행
    void Update()
    {
        currentState?.Execute();
    }

    // 상태 전환
    public void ChangeState(IState nextState)
    {
        currentState?.Exit();
        currentState = nextState;
        currentState.Enter();
    }

    // bool 타입 파라미터를 이용하는 애니메이션 전환
    public void StartAnime(int parameterHash)
    {
        animator.SetBool(parameterHash, true);
    }
    public void StopAnime(int parameterHash)
    {
        animator.SetBool(parameterHash, false);
    }

    // 트리거 파라미터로 동작하는 애니메이션 호출(죽음)
    public void StartAnimeTrigger(int parameterHash)
    {
        animator.SetTrigger(parameterHash);
    }
}