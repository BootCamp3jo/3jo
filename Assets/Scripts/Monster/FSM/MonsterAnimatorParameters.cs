using UnityEngine;

public class MonsterAnimatorParameters
{
    // 애니메이터 파라미터 이름들
    #region Animatior Transition Parameters Name
    string idleName = "Idle";
    string moveName = "Move";
    string deathName = "Death";
    // 공격 레이어 트랜지션 조건으로 들어갈 파라미터 이름
    string attackLayerName = "@Attack";
    // 해당 파라미터는 bool이 아닌 int로 공격 패턴 갈래를 표현
    string attackName = "Attack";
    string attackCountRemainName = "AttackCountRemain";
    #endregion

    // 이름들을 해시로 변환해둔 값
    #region Animatior Transition Parameters Hash
    public int idleHash { get; private set; }
    public int moveHash { get; private set; }
    public int deathHash { get; private set; }
    public int attackLayerHash { get; private set; }
    public int attackHash { get; private set; }
    public int attackCountRemainHash { get; private set; }
    #endregion

    // 생성자로 파라미터 이름을 해시로 변환
    public MonsterAnimatorParameters()
    {
        idleHash = Animator.StringToHash(idleName);
        moveHash = Animator.StringToHash(moveName);
        deathHash = Animator.StringToHash(deathName);
        attackLayerHash = Animator.StringToHash(attackLayerName);
        attackHash = Animator.StringToHash(attackName);
        attackCountRemainHash = Animator.StringToHash(attackCountRemainName);
    }
}
