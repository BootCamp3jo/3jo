using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_", menuName = "Data/Pattern_Range_Random")]
public class PatternData_Range_Random2 : PatternData_Range, IRandomAtkCount, IRandomPosRange
{
   [field:SerializeField] public int2 randomAtkCount { get; set; }
   [field: SerializeField] public float2x2 patternRange { get; set; }
}
