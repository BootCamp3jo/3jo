using UnityEngine;
using System;

[CreateAssetMenu(fileName ="Monster", menuName ="Data/Monster")]
public class MonsterData : ScriptableObject
{
    // ���� ���Ϳ��� ������ �־�� �ұ�?
    // �̸�
    [field: SerializeField] public string monsterName { get; private set; } 
    // �����/���� ������ ���� �Ҽ����� �ʿ����� �𸣴� �̸� float ������ ����
    // ü��
    [field: SerializeField] public float hp { get; private set; }
    // �⺻ ���ݷ� >> �̸� ������� ���ϸ��� ����� �ִ� ���(���� ���� ǥ���� ����)
    [field: SerializeField] public float atk { get; private set; }
    // ����..�� �̹� ���� �Ⱓ�� ª���� �н�(ü�¸����� �뷱�� �����ϱ⿡�� ������ ��)
    // ��� �� ����ϴ� ����
    [field: SerializeField] DropData dropData; 
}

[Serializable]
public class DropData
{
    int exp; // ����ġ
    DropItem[] dropItems; // ��� ������ Ǯ
}

// !!! �������� ������� �� �ۼ��ϱ�!
[Serializable]
public class DropItem
{
    // ������ ����
    // ��� ����
}