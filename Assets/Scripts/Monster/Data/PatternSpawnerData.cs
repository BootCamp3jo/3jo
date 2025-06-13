using UnityEngine;

[CreateAssetMenu(fileName = "PatternSpawner_", menuName = "Data/PatternSpawner")]
public class PatternSpawnerData : ScriptableObject
{
    // 패턴 이펙트 하나의 프리팹
    [field: SerializeField] public Transform effectPrefab { get; private set; }
    // 몇개의 이펙트를 사용할지
    [field: SerializeField] public int effectNum { get; private set; }
}
