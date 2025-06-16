using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_", menuName = "Data/Pattern_Range")]
public class PatternData_Range : PatternData
{
    // 경고가 끝난 후 공격이 지속되는 시간
    [field: SerializeField] public float attackTime { get; private set; }
}
