using UnityEngine;

public class Range : MonoBehaviour, ISlowAble
{
    public float atkReadyTime { get; set; }
    Vector3 scaleChangePerFrame;
    PatternRange pattern;
    bool isAtkStart;

    private void Awake()
    {
        pattern = GetComponentInParent<PatternRange>(true);
    }

    private void OnEnable()
    {
        isAtkStart = false;
        // 범위 표시 초기화
        transform.localScale = Vector3.zero;
        // 프레임 당 스케일 변화량 초기화(atkReadyTime의 변동이 있는 기술 대비)
        scaleChangePerFrame = Vector3.one / atkReadyTime;
    }

    private void Update()
    {
        // 공격 범위에 맞게 점점 커지는 경고 영역
        if (isSlowed)
            transform.localScale += scaleChangePerFrame * GameManager.Instance.slowRate * Time.deltaTime;
        else
            transform.localScale += scaleChangePerFrame * Time.deltaTime;
        // 공격 범위가 다 채워졌다면
        if (transform.localScale.x >= 1 && !isAtkStart)
        {
            isAtkStart = true;
            // 범위 밖으로 벗어나지 않게
            transform.localScale = Vector3.one;
            // 공격!
            pattern.rangeAttack.StartAttack();
            // 임팩트 애니메이션 재생
            pattern.changeEffect.ChangeEffectAnime(pattern.changeEffect.impactHash);
        }
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
}
