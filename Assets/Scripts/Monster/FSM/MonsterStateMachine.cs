// 몬스터 FSM 각 상태 클래스에서 공통으로 구현할 구조를 미리 정의
public interface IState
{
    void Enter(); // 해당 상태에 진입할 때 수행할 내용
    void Execute(); // 해당 상태 도중 수행할 내용(Update)
    void ExecutePhysically(); // 해당 상태 도중 수행할 내용(FixedUpdate)
    void Exit(); // 해당 상태에서 나갈 때 수행할 내용
}

public class MonsterStateMachine
{
    // 상태 중에 몬스터의 메서드를 사용할 수 있기에 연결!
    public MonsterBase monster { get; private set; }
    public MonsterStateMachine(MonsterBase monster)
    {
        this.monster = monster;

        // 각 상태 초기화
        idleState = new(this);
        deathState = new(this);
        attackState = new(this);
        moveState = new(this);

        // 처음은 대기
        ChangeState(idleState);
    }

    // 현재 어떤 상태인지 넣어둘 변수
    private IState currentState;

    // 애니메이터에서 사용할 파라미터의 해시를 가져올 수 있다!
    public MonsterAnimatorParameters AnimatorParameters = new();

    // 각 상태를 정의해주기(필요한 상태 추가)
    public MonsterState_Idle idleState { get; private set; }
    public MonsterState_Death deathState { get; private set; }
    public MonsterState_Attack attackState { get; private set; }
    public MonsterState_Move moveState { get; private set; }

    // 현재 상태의 Excute()를 지속적으로 수행!
    public void Execute()
    {
        currentState?.Execute();
    }

    public void ExecutePhysically()
    {
        currentState?.ExecutePhysically();
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
        monster.animator.SetBool(parameterHash, true);
    }
    public void StopAnime(int parameterHash)
    {
        monster.animator.SetBool(parameterHash, false);
    }

    // 트리거 파라미터로 동작하는 애니메이션 호출(죽음)
    public void SetAnimeTrigger(int parameterHash)
    {
        monster.animator.SetTrigger(parameterHash);
    }
}