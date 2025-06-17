using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    // 범위 공격이 떨어지는 지점
    PointType endPointType;
    float2x2 range;

    public LayerMask targetLayer;

    Collider2D trigger;

    // 공격할 위치
    public Vector3 atkPoint { get; set; }

    // 공격력은 언제 받아둘까? 디버프 같은 게 있을지 모르니 쓸 때 받는걸로
    public float Damage { get; set; }
    // 공격 지속 시간
    public float AttackRemainTime {  get; set; }

    SpriteRenderer[] spriteRenderers;

    // 충격파 효과
    ShockWaveEffect waveEffect;

    private void Awake()
    {
        trigger = GetComponent<Collider2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        waveEffect = GetComponent<ShockWaveEffect>();
        PatternData patternData = GetComponentInParent<Pattern>(true).patternData;
        if (patternData != null) 
        {
            endPointType = patternData.endPointType;

            IRandomPosRange randomPosRange = patternData as IRandomPosRange;
            if(randomPosRange != null)
                range = randomPosRange.patternRange;
        }
    }
    private void OnEnable()
    {
        // 트리거를 끄기
        trigger.enabled = false;
        // 경고 범위가 다시 보이게끔
        ChangeMarkEnable(true);
        // 목표 위치로 공격 범위 경고 오브젝트 이동
        switch (endPointType)
        {
            case PointType.Player:
                transform.position = atkPoint = PlayerManager.Instance.playerPrefab.transform.position;
                break;
            case PointType.Enemy:
                transform.position = atkPoint = GameManager.Instance.boss.transform.position;
                break;
            case PointType.Random:
                transform.position = atkPoint = new Vector2(UnityEngine.Random.Range(range.c0.x, range.c1.x), UnityEngine.Random.Range(range.c0.y, range.c1.y));
                break;
            case PointType.Random_AroundPlayer:
                transform.position = atkPoint = (Vector2)PlayerManager.Instance.playerPrefab.transform.position + new Vector2(UnityEngine.Random.Range(range.c0.x, range.c1.x), UnityEngine.Random.Range(range.c0.y, range.c1.y));
                break;
            case PointType.Random_AroundEnemy:
                transform.position = atkPoint = (Vector2)GameManager.Instance.boss.transform.position + new Vector2(UnityEngine.Random.Range(range.c0.x, range.c1.x), UnityEngine.Random.Range(range.c0.y, range.c1.y));
                break;
        }
    }

    // 트리거가 켜지면, 그 안의 타겟에 1번만 피해를 입힘
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 머지 이후 플레이어에 25번 Player 레이어 할당하기 !!!
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            var player = collision.GetComponent<Player>();
            player.OnHitByEnemy(Damage);
        }
    }


    Coroutine coroutine_atk;
    public void Attack()
    {
        if(coroutine_atk != null)
            StopCoroutine(coroutine_atk);
        coroutine_atk = StartCoroutine(AttackAndEnd());
    }

    IEnumerator AttackAndEnd()
    {
        // 연출
        if (waveEffect != null)
            waveEffect.TriggerShockWave(transform);
        // 트리거를 켜서 공격!
        trigger.enabled = true;
        AudioManager.instance.PlaySFX(SFXType.Boom, 0.8f, 1f);
        // 공격 범위가 남아있으면 안되니 영역 표시 오브젝트 비활성화
        ChangeMarkEnable(false);
        // 지속 시간 동안 영역에 대미지
        yield return new WaitForSeconds(AttackRemainTime);
        // 다시 트리거를 끄기
        trigger.enabled = false;

        Destroy(transform.parent.gameObject);
    }

    void ChangeMarkEnable(bool isEnable)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].enabled = isEnable;
        }
    }
}
