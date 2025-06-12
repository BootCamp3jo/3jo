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
    // 현재 어떤 상태인지 넣어둘 변수
    private IState currentState;

    public Animator animator { get; private set; }
    // 타겟이 될 플레이어의 레이어
    [field:SerializeField] public LayerMask player { get; private set;}

    // 각 상태를 정의해주기(필요한 상태 추가)
    MonsterState_Idle idleState;
    MonsterState_Death deathState;

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
}