using System.Collections;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public LayerMask targetLayer;
    public PatternData patternData; 

    Collider2D trigger;

    // 공격할 위치
    public Vector3 atkPoint { get; set; }

    // 공격력은 언제 받아둘까? 디버프 같은 게 있을지 모르니 쓸 때 받는걸로 !!!
    public float Damage { get; set; }

    private void Awake()
    {
        trigger = GetComponent<Collider2D>();
    }
    private void OnEnable()
    { 
        // 우선 플레이어 위치로 고정 !!!
        atkPoint = PlayerManager.Instance.playerPrefab.transform.position;
        // 목표 위치로 공격 범위 경고 오브젝트 이동
        transform.position = atkPoint;
    }

    // 트리거가 켜지면, 그 안의 타겟에 1번만 피해를 입힘
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 머지 이후 플레이어에 25번 Player 레이어 할당하기 !!!
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            // 플레이어 피격... 어디있을까요? 머지 이후 찾아서 연결하기!
        }
    }

    public void Attack()
    {
        StartCoroutine(AttackAndEnd());
    }

    IEnumerator AttackAndEnd()
    {
        // 트리거를 켜서 공격!
        trigger.enabled = true;
        // 1프레임의 찰나 동안 영역에 대미지
        yield return null;
        // 다시 트리거를 끄고
        trigger.enabled = false;
        // 공격 범위가 남아있으면 안되니 영역 표시 오브젝트 비활성화
        gameObject.SetActive(false);
    }
}
