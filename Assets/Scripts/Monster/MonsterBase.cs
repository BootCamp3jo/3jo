using UnityEngine;

public abstract class MonsterBase : MonoBehaviour
{
    [field: SerializeField] public MonsterData monsterData;

    // Ÿ���� �� �÷��̾��� ���̾�
    [field: SerializeField] public LayerMask player { get; private set; }

    public MonsterStateMachine monsterStateMachine { get; private set; }

    // ���� ü��
    float hpCurrent;
    // ���� ��Ÿ��(���� ���ϸ��� ��Ÿ���� �޶� ��� ���� ���ÿ� ���ֱ�!!!)
    public float atkDelay { get; set; }

    private void Awake()
    {
        monsterStateMachine = new MonsterStateMachine(this);
    }

    private void Start()
    {
        // ������ �� ���� HP�� �ִ� HP��
        hpCurrent = monsterData.hp;
    }

    public void ChangeAtkDelay(float newDelay)
    {
        atkDelay = newDelay;
    }

    // ����� ���
    public void GetDamage(float damage)
    {
        if (hpCurrent <= 0) return;
        hpCurrent = Mathf.Max(hpCurrent-damage, 0);
        // �ǰ� ����Ʈ�� �ִٸ� ���⿡!
        if (hpCurrent <= 0)
            Dead();
    }

    // ��� ó��
    void Dead()
    {
        // ���� ���
        monsterStateMachine.ChangeState(monsterStateMachine.deathState);
    }
}
