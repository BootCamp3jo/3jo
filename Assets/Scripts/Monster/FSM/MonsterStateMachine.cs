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
    // ���� � �������� �־�� ����
    private IState currentState;

    public Animator animator { get; private set; }
    // Ÿ���� �� �÷��̾��� ���̾�
    [field:SerializeField] public LayerMask player { get; private set;}

    // �� ���¸� �������ֱ�(�ʿ��� ���� �߰�)
    MonsterState_Idle idleState;
    MonsterState_Death deathState;

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
}