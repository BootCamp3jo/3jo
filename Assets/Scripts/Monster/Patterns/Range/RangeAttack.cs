using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    [SerializeField] Transform WarningRange;
    public Vector3 atkPoint;
    private void OnEnable()
    {
        // 목표 위치로 공격 범위 경고 오브젝트 이동
        WarningRange.transform.position = atkPoint;
    }
}
