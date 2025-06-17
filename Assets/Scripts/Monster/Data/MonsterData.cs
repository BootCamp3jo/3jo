using UnityEngine;
using System;

[CreateAssetMenu(fileName ="Monster", menuName ="Data/Monster")]
public class MonsterData : ScriptableObject
{
    // 보스 몬스터에는 무엇이 있어야 할까?
    // 이름
    [field: SerializeField] public string monsterName { get; private set; } 
    // 디버프/버프 등으로 인한 소숫점이 필요할지 모르니 미리 float 형으로 선언
    // 최대 체력
    [field: SerializeField] public float hpMax { get; private set; }
    // 기본 공격력 >> 이를 기반으로 패턴마다 계수가 있는 방식(강한 패턴 표현에 용이)
    [field: SerializeField] public float atk { get; private set; }
    // 이동 속도
    [field: SerializeField] public float moveSpeed { get; private set; }
    // 방어력..은 이번 팀프 기간이 짧으니 패스(체력만으로 밸런싱 조정하기에도 빠듯할 듯)
    // 공격 딜레이는 패턴 데이터에

    // 사망 시 드랍하는 내용
    [field: SerializeField] public DropData dropData; 
}

[Serializable]
public class DropData
{
    public int exp; // 경험치
    public DropItem[] dropItems; // 드랍 아이템 풀
}

// !!! 아이템이 만들어진 후 작성하기!
[Serializable]
public class DropItem
{
    // 아이템 종류
    // 드랍 수량
}