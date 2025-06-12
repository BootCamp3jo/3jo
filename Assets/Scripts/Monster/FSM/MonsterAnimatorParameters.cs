using UnityEngine;

public class MonsterAnimatorParameters
{
    // �ִϸ����� �Ķ���� �̸���
    #region Animatior Transition Parameters Name
    [field: SerializeField] public string idleName { get; private set; } = "Idle";
    [field: SerializeField] public string moveName { get; private set; } = "Move";
    [field: SerializeField] public string deathName { get; private set; } = "Death";
    // ���� ���̾� Ʈ������ �������� �� �Ķ���� �̸�
    [field: SerializeField] public string attackLayerName { get; private set; } = "@Attack";
    // ���� ���� �������� �ٸ� �� �ֱ⿡ �̿� �����ϱ� ���� �迭�� ����
    // �迭 �ε����� ���� 0 ~ n-1������ (n = ���� ��)
    [field: SerializeField] public string[] attckNames { get; private set; } = new string[1] { "Attack0" };
    #endregion

    // �̸����� �ؽ÷� ��ȯ�ص� ��
    #region Animatior Transition Parameters Hash
    public int idleHash { get; private set; }
    public int moveHash { get; private set; }
    public int deathHash { get; private set; }
    public int attackLayerHash { get; private set; }
    public int[] attckHashs { get; private set; }
    #endregion

    // �����ڷ� �Ķ���� �̸��� �ؽ÷� ��ȯ
    public MonsterAnimatorParameters()
    {
        idleHash = Animator.StringToHash(idleName);
        moveHash = Animator.StringToHash(moveName);
        deathHash = Animator.StringToHash(deathName);
        attackLayerHash = Animator.StringToHash(attackLayerName);

        attckHashs = new int[attckNames.Length];
        for (int i = 0; i < attckNames.Length; i++)
        {
            attckHashs[i] = Animator.StringToHash(attckNames[i]);
        }
    }
}
