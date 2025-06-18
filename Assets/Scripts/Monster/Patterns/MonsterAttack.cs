using Unity.Mathematics;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    // 범위 공격이 떨어지는 지점
    PointType endPointType = PointType.Enemy; // patternData 없이 쓸 때를 대비해 Enemy를 디폴트 값으로
    float2x2 range;
    // 공격할 위치
    protected Vector3 atkPoint { get; set; }

    // 플레이어만 타겟이므로 불필요. 버그만 낳을 뿐
    //public LayerMask targetLayer;

    protected ShockWaveEffect waveEffect;
    // 공격력은 언제 받아둘까? 디버프 같은 게 있을지 모르니 쓸 때 받는걸로.. 했는데 디버프가 없기에 Pattern을 거치지 않고 직접 써줄 수 있게 직렬화
    [field:SerializeField] public float Damage { get; set; }

    public PatternData patternData { get; set; }

    protected virtual void Awake()
    {

    }

    protected virtual void OnEnable()
    {
        Pattern pattern = GetComponentInParent<Pattern>(true);
        if (pattern != null)
        {
            PatternData patternData = pattern.patternData;
            if (patternData != null)
            {
                endPointType = patternData.endPointType;

                IRandomPosRange randomPosRange = patternData as IRandomPosRange;
                if (randomPosRange != null)
                    range = randomPosRange.patternRange;

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
        }
    }

    private void Start()
    {
        // 기존 범위공격에서 쓰던 쇼크웨이브, sfx는 공격에 코루틴을 쓰지 않게됨에 따라 여기로 이동 !!! 쓸 때 주석 풀고 값 조정해주기
        //// 연출
        //if (waveEffect != null)
        //    waveEffect.TriggerShockWave(transform);
        //AudioManager.instance.PlaySFX(SFXType.Boom, 0.8f, 1f);
    }

    // 트리거가 켜지면, 그 안의 타겟에 1번만 피해를 입힘
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
            player.OnHitByEnemy(Damage);
    }

    // 패턴이 끝나고 파괴
    public void DestroyPattern()
    {
        GameObject pattern = GetComponentInParent<Pattern>().gameObject;
        if(pattern)
            Destroy(pattern);
    }
}
