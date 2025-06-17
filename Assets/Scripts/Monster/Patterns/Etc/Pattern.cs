using UnityEngine;

public class Pattern : EffectController
{
    [field:SerializeField] public PatternData patternData { get; private set; }
    public ChangeEffect changeEffect { get; private set; }

    public MonsterAttack monsterAttack { get; private set; }

    protected virtual void Awake()
    {
        monsterAttack = GetComponentInChildren<MonsterAttack>(true);
        // 대미지 부여
        monsterAttack.Damage = patternData.atkCoefficient * GameManager.Instance.boss.monsterData.atk;

        // 패턴의 라이프 사이클을 기억
        lifeTime = patternData.lifeTime;
        changeEffect = GetComponentInChildren<ChangeEffect>(true);
        // 패턴을 비활성화 해둬서 위의 값을 받아올 수 없어 처음 실행 때 위치 에러 발생 >> Awake()만 실행하고 비활성화하게끔 구조 개편
       // LifeEnd();
    }
}
