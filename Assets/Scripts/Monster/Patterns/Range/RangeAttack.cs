using Unity.Mathematics;
using UnityEngine;

public class RangeAttack : MonoBehaviour, ISlowAble
{
    // 범위 공격이 떨어지는 지점
    PointType endPointType;
    float2x2 range;

    Collider2D trigger;

    // 공격할 위치
    public Vector3 atkPoint { get; set; }

    // 공격력은 언제 받아둘까? 디버프 같은 게 있을지 모르니 쓸 때 받는걸로
    public float Damage { get; set; }
    // 공격 지속 시간 최대치
    public float AttackRemainTime { get; set; }
    // 남은 공격 지속 시간
    float atkRemainTmp;

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
        atkRemainTmp = AttackRemainTime;
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

    private void Update()
    {
        // 트리거가 켜졌다면, 공격 지속 시간 동안(슬로우 영향도 받음) 지속 후 트리거 꺼서 공격 종료
        if(trigger.enabled)
        {
            if(isSlowed)
                atkRemainTmp -= Time.deltaTime * GameManager.Instance.slowRate;
            else
                atkRemainTmp -= Time.deltaTime;
            if (atkRemainTmp <= 0)
            {
                // 다시 트리거를 끄기
                trigger.enabled = false;
            }
        }
    }

    // 트리거가 켜지면, 그 안의 타겟에 피해를 입힘
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Player player))
        {
            player.OnHitByEnemy(Damage);
        }
    }

    // 공격 시작
    public void StartAttack()
    {
        // 연출
        if (waveEffect != null)
            waveEffect.TriggerShockWave(transform);
        // 트리거를 켜서 공격!
        trigger.enabled = true;        
        // 공격 범위가 남아있으면 안되니 영역 표시 오브젝트 비활성화
        ChangeMarkEnable(false);
    }


    // 슬로우/해제 관련 구현
    // 슬로우 중인지 판정하기 위한 프로퍼티(없으면 감속이 된 건지 아닌지 판정할 수 없기에)
    public bool isSlowed { get; set; } = false;
    // 감속 시작!
    public void StartSlow()
    {
        isSlowed = true;
    }
    // 감속 끝!
    public void StopSlow()
    {
        isSlowed = false;
    }

    void ChangeMarkEnable(bool isEnable)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].enabled = isEnable;
        }
    }
}
