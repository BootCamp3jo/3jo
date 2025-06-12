using UnityEngine;

// ���� FSM �� ���� Ŭ�������� �������� ������ ������ �̸� ����
public interface IState
{
    void Enter(); // �ش� ���¿� ������ �� ������ ����
    void Execute(); // �ش� ���� ���� ������ ����
    void Exit(); // �ش� ���¿��� ���� �� ������ ����
}

public class MonsterStateMachine : MonoBehaviour
{
    // ���� �߿� ������ ������ �� �ֱ⿡ ����!
    public MonsterBase monster {get; private set;}
    public MonsterStateMachine(MonsterBase monster) => this.monster = monster;

    // ���� � �������� �־�� ����
    private IState currentState;

    // �ִϸ����Ϳ� �Ķ����
    Animator animator;
    public MonsterAnimatorParameters AnimatorParameters = new();

    // �� ���¸� �������ֱ�(�ʿ��� ���� �߰�)
    public MonsterState_Idle idleState { get; private set; }
    public MonsterState_Death deathState { get; private set; }

    void Start()
    {
        animator = GetComponent<Animator>();

        // �� ���� �ʱ�ȭ
        idleState = new MonsterState_Idle(this);
        deathState = new MonsterState_Death(this);

        // ó���� ���
        ChangeState(idleState);
    }

    // Execute()�� �̰����� ��� ����
    void Update()
    {
        currentState?.Execute();
    }

    // ���� ��ȯ
    public void ChangeState(IState nextState)
    {
        currentState?.Exit();
        currentState = nextState;
        currentState.Enter();
    }

    // bool Ÿ�� �Ķ���͸� �̿��ϴ� �ִϸ��̼� ��ȯ
    public void StartAnime(int parameterHash)
    {
        animator.SetBool(parameterHash, true);
    }
    public void StopAnime(int parameterHash)
    {
        animator.SetBool(parameterHash, false);
    }

    // Ʈ���� �Ķ���ͷ� �����ϴ� �ִϸ��̼� ȣ��(����)
    public void StartAnimeTrigger(int parameterHash)
    {
        animator.SetTrigger(parameterHash);
    }
}