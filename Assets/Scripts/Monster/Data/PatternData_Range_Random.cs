using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern_", menuName = "Data/Pattern_Range_Random")]
public class PatternData_Range_Random2 : PatternData_Range, IRandomAtkCount, IRandomPosRange
{
    // 랜덤 공격 횟수
    [field:SerializeField] public int2 randomAtkCount { get; set; }
   // 패턴 설치 위치에 랜덤성을 부여하기 위한 범위.. 긴 한데 그냥 float 하나로 해도 될지도? 상세하게 표현하려면 이래야 맞지만
   [field: SerializeField] public float2x2 patternRange { get; set; }
}
