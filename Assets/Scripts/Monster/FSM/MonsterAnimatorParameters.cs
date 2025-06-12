using UnityEngine;

public class MonsterAnimatorParameters
{
    // �ִϸ����� �Ķ���� �̸���
    #region Animatior Transition Parameters Name
    [field: SerializeField] public string idleParameterName { get; private set; } = "Idle";
    [field: SerializeField] public string moveParameterName { get; private set; } = "Move";
    [field: SerializeField] public string deathParameterName { get; private set; } = "Death";
    // ���� ���̾� Ʈ������ �������� �� �Ķ���� �̸�
    [field: SerializeField] public string attackLayerName { get; private set; } = "@Attack";
    // ���� ���� �������� �ٸ� �� �ֱ⿡ �̿� �����ϱ� ���� �迭�� ����
    // �迭 �ε����� ���� 0 ~ n-1������ (n = ���� ��)
    [field: SerializeField] public string[] attckParameterNames { get; private set; } = new string[1] { "Attack0" };
    #endregion

    // �̸����� �ؽ÷� ��ȯ�ص� ��
    #region Animatior Transition Parameters Hash
    public int idleParameterHash { get; private set; }
    public int moveParameterHash { get; private set; }
    public int deathParameterHash { get; private set; }
    public int attackLayerHash { get; private set; }
    public int[] attckParameterHashs { get; private set; }
    #endregion

    // �����ڷ� �Ķ���� �̸��� �ؽ÷� ��ȯ
    public MonsterAnimatorParameters()
    {
        idleParameterHash = Animator.StringToHash(idleParameterName);
        moveParameterHash = Animator.StringToHash(moveParameterName);
        deathParameterHash = Animator.StringToHash(deathParameterName);
        attackLayerHash = Animator.StringToHash(attackLayerName);

        attckParameterHashs = new int[attckParameterNames.Length];
        for (int i = 0; i < attckParameterNames.Length; i++)
        {
            attckParameterHashs[i] = Animator.StringToHash(attckParameterNames[i]);
        }
    }
}
