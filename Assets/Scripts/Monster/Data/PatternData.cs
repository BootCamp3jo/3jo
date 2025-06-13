using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_", menuName = "Data/Pattern")]
public class PatternData : ScriptableObject
{
    // 각 몬스터 패턴에는 무엇이 공통적으로 필요할까?
    // 패턴의 프리팹(탄환 하나 or 장판 하나 or 근거리 공격 이펙트 하나)
    [field: SerializeField] public GameObject prefab { get; private set; }
    // 프리팹을 어떻게 얼마나 배치할지 정하는 메서드를 여기에 대입
    [field: SerializeField] public Action Arrange { get; private set; }
    // 공격력 계수(몬스터 공격력 * 계수 = 최종 대미지, 단, 플레이어가 HP를 int로 쓴다면 반올림)
    [field: SerializeField] public float atkCoefficient { get; private set; }
    // 해당 패턴이 공격 가능한 거리(최소, 최대)
    [field: SerializeField] public float rangeMin { get; private set; }
    [field: SerializeField] public float rangeMax { get; private set; }
    // 해당 패턴 이후의 공격 딜레이
    [field: SerializeField] public float delay { get; private set; }
}

// 어느 상황에 어떤 패턴을 쓰는지 어떻게 정할까?
// 플레이어와의 거리? >> 어느 정도 패턴 별로 겹치는 범위를 두어 이에 공통으로 여건이 되는 패턴들이 있다면 이 중 랜덤으로 하는 것이 이상적일 듯
// 각 패턴을 순서대로 반복해서? >> 이렇게 쓰는 게임도 봤지만 단조로울 수 있음
// 올 랜덤? >> 광범위한 패턴만 있는 보스라면 상관이 없어보이긴 한데, 그렇지 않다면 상황에 맞지 않는 패턴이 나갈 수 있음

// 라이프타임은.. 필요없을지도?
// 탄환이라면 플레이어/맵 가장자리 부딪히면 사라지게끔
// 근거리, 장판은 애니메이션이 끝나면 사라지게끔
