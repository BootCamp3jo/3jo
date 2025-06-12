using UnityEngine;

public class MonsterAnimatorParameters
{
    // 애니메이터 파라미터 이름들
    #region Animatior Transition Parameters Name
    [field: SerializeField] public string idleParameterName { get; private set; } = "Idle";
    [field: SerializeField] public string moveParameterName { get; private set; } = "Move";
    [field: SerializeField] public string deathParameterName { get; private set; } = "Death";
    // 공격 레이어 트랜지션 조건으로 들어갈 파라미터 이름
    [field: SerializeField] public string attackLayerName { get; private set; } = "@Attack";
    // 패턴 수가 보스마다 다를 수 있기에 이에 대응하기 위해 배열로 선언
    // 배열 인덱스에 따라 0 ~ n-1번까지 (n = 패턴 수)
    [field: SerializeField] public string[] attckParameterNames { get; private set; } = new string[1] { "Attack0" };
    #endregion

    // 이름들을 해시로 변환해둔 값
    #region Animatior Transition Parameters Hash
    public int idleParameterHash { get; private set; }
    public int moveParameterHash { get; private set; }
    public int deathParameterHash { get; private set; }
    public int attackLayerHash { get; private set; }
    public int[] attckParameterHashs { get; private set; }
    #endregion

    // 생성자로 파라미터 이름을 해시로 변환
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
