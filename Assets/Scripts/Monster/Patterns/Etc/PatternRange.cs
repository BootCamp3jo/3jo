public class PatternRange : Pattern
{
    public Range range { get; private set; }
    public RangeAttack rangeAttack { get; private set; }

    protected override void Awake()
    {
        range = GetComponentInChildren<Range>(true);
        rangeAttack = GetComponentInChildren<RangeAttack>(true);
        if (range != null && rangeAttack != null)
        {
            // 범위 공격에 대미지 부여
            rangeAttack.Damage = patternData.atkCoefficient * GameManager.Instance.boss.monsterData.atk;
            // 공격 유지 시간 부여
            rangeAttack.AttackRemainTime = (patternData as PatternData_Range).attackTime;
            // 패터의 생명주기에 공격 유지 시간을 빼서 그 시간만큼 패턴 경고를 주게
            range.atkReadyTime = patternData.lifeTime - rangeAttack.AttackRemainTime;
        }
        base.Awake();
    }
}
