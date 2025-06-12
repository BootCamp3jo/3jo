using UnityEngine;

public class MonsterAnimatorParameters
{
    // 애니메이터 파라미터 이름들
    #region Animatior Transition Parameters Name
    [field: SerializeField] public string idleName { get; private set; } = "Idle";
    [field: SerializeField] public string moveName { get; private set; } = "Move";
    [field: SerializeField] public string deathName { get; private set; } = "Death";
    // 공격 레이어 트랜지션 조건으로 들어갈 파라미터 이름
    [field: SerializeField] public string attackLayerName { get; private set; } = "@Attack";
    // 패턴 수가 보스마다 다를 수 있기에 이에 대응하기 위해 배열로 선언
    // 배열 인덱스에 따라 0 ~ n-1번까지 (n = 패턴 수)
    [field: SerializeField] public string[] attckNames { get; private set; } = new string[1] { "Attack0" };
    #endregion

    // 이름들을 해시로 변환해둔 값
    #region Animatior Transition Parameters Hash
    public int idleHash { get; private set; }
    public int moveHash { get; private set; }
    public int deathHash { get; private set; }
    public int attackLayerHash { get; private set; }
    public int[] attckHashs { get; private set; }
    #endregion

    // 생성자로 파라미터 이름을 해시로 변환
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
