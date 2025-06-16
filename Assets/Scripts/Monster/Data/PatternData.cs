using UnityEngine;
using Unity.Mathematics;

// 패턴 수가 랜덤인 패턴들에 추가
public interface IRandomAtkCount
{
    public int2 randomAtkCount { get; set; }
}

public interface IRandomPosRange
{
    public float2x2 patternRange {  get; set; }
}

public enum PatternType
{
    Melee, // 근접
    Bullet, // 탄
    Range, // 범위
}

// 패턴의 시작 위치가 어딘지 나타내는 타입
public enum PointType
{
    Player,
    Enemy,
    Random,
    Random_AroundEnemy,
    Random_AroundPlayer,
    RangeCenter,
}

[CreateAssetMenu(fileName = "Pattern_", menuName = "Data/Pattern")]
public class PatternData : ScriptableObject
{
    // 각 몬스터 패턴에는 무엇이 공통적으로 필요할까?
    // 패턴 타입
    public PatternType patternType;

    // 시작, 끝 위치
    public PointType startPointType, endPointType;

    // 한번에 나가는 이펙트..그러니까 1번의 애니메이션 재생 당 나가는 공격의 수
    [field: SerializeField] public int atkNumForOnce { get; private set; }

    // 공격력 계수(몬스터 공격력 * 계수 = 최종 대미지, 단, 플레이어가 HP를 int로 쓴다면 반올림)
    [field: SerializeField] public float atkCoefficient { get; private set; }
    // 해당 패턴이 공격 가능한 거리(최소, 최대)
    [field: SerializeField] public float2 range { get; private set; }
    // 해당 패턴 이후의 공격 딜레이
    [field: SerializeField] public float delay { get; private set; }
    // 패턴 지속 시간
    [field: SerializeField] public float lifeTime {  get; private set; }
}

// 어느 상황에 어떤 패턴을 쓰는지 어떻게 정할까?
// 플레이어와의 거리? >> 어느 정도 패턴 별로 겹치는 범위를 두어 이에 공통으로 여건이 되는 패턴들이 있다면 이 중 랜덤으로 하는 것이 이상적일 듯
// 각 패턴을 순서대로 반복해서? >> 이렇게 쓰는 게임도 봤지만 단조로울 수 있음
// 올 랜덤? >> 광범위한 패턴만 있는 보스라면 상관이 없어보이긴 한데, 그렇지 않다면 상황에 맞지 않는 패턴이 나갈 수 있음

// 라이프타임은.. 필요없을지도?
// 탄환이라면 플레이어/맵 가장자리 부딪히면 사라지게끔
// 근거리, 장판은 애니메이션이 끝나면 사라지게끔
