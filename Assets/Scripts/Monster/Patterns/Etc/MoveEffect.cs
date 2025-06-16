using UnityEngine;

public enum MoveType
{
    Linear,
    Curve,
}

// 공격 이펙트가 이동할 때 사용(탄환, 장판 범위 표시 후 날아가는 투사체 등)
public class MoveEffect : MonoBehaviour, ISlowAble
{
    Transform rangeCenter;
    PointType startPoint_ref, endPoint_ref;

    #region Move
    public MoveType moveType;
    float rangeMax;
    // 이동에 소요되는 시간
    public float moveTime = 3f;
    // 곡선일 때 중점의 높이
    public float heigthOfPathCurve = 3;
    float loopCount, loopCountMax;
    #endregion

    #region SelfRotate
    // 회전 여부
    [field:SerializeField] public bool isSelfRotating_Default { get; private set; }
    bool isSelfRotating;
    // 회전 속도
    [field: SerializeField] public float rotationSpeed { get; private set; }
    // 회전 속도에 따른 각속도
    Quaternion rotationPerFixedFrame;
    #endregion

    Vector3 startPoint, midPoint, endPoint;

    float progressRate = 0f;
    float totalLength;
    bool isMoving = false;

    Vector3 velocityTmp;

    private void Awake()
    {

        PatternRange patternRange = GetComponentInParent<PatternRange>(true);
        if (patternRange != null)
        {
            rangeCenter = patternRange.rangeAttack.transform;
        }
        PatternData patternData = GetComponentInParent<Pattern>(true).patternData;
        startPoint_ref = patternData.startPointType;
        endPoint_ref = patternData.endPointType;
        rangeMax = patternData.range.y;

        loopCountMax = moveTime / Time.fixedDeltaTime;
    }


    void OnEnable()
    {
        // 다시 이동할 수 있도록 초기화
        isMoving = true;
        loopCount = 0;
        progressRate = 0f;
        isSelfRotating = isSelfRotating_Default;
        //transform.position = startPoint;

        switch (startPoint_ref)
        {
            case PointType.Player:
                startPoint = PlayerManager.Instance.player.transform.position;
                break;
            case PointType.Enemy:
                startPoint = GameManager.Instance.boss.transform.position;
                break;
            case PointType.Random:
            case PointType.RangeCenter:
            case PointType.Random_AroundEnemy:
            case PointType.Random_AroundPlayer:
                startPoint = rangeCenter != null ? rangeCenter.position : transform.position;
                break;
            default:
                break;
        }

        switch (endPoint_ref)
        {
            case PointType.Player:
                endPoint = PlayerManager.Instance.player.transform.position;
                break;
            case PointType.Enemy:
                endPoint = GameManager.Instance.boss.transform.position;
                break;
            case PointType.Random:
            case PointType.RangeCenter:
            case PointType.Random_AroundEnemy:
            case PointType.Random_AroundPlayer:
                if (rangeCenter)
                    endPoint = rangeCenter.position;
                break;
        }

        // 발사 당시 적 위치 노리게끔
        
        
        switch (moveType)
        {
            case MoveType.Linear:
                totalLength = Vector2.Distance(startPoint, endPoint);
                break;
            case MoveType.Curve:
                MidPoint(startPoint, endPoint, heigthOfPathCurve);
                totalLength = EstimateCurveLength(20);
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        // Time.fixedDeltaTime 값은 고정(보통 0.02초)이기에 매번 같은 값으로 회전 >> 곱하지 않고 그냥 rotationSpeed만으로 조정
        rotationPerFixedFrame = Quaternion.Euler(Vector3.forward * rotationSpeed);
    }

    private void FixedUpdate()
    {
        // 이동이 끝나면 회전도 끝나게끔
        if (!isMoving) return;
        // 이동
        Move();
        // 자전
        SelfRotate();
    }

    // 곡선의 중점
    Vector3 MidPoint(Vector3 a, Vector3 b, float heightOffset = 0f)
    {
        // 시작과 끝의 중간지점에서 y축 방향으로 원하는 높이만큼 올라간 지점
        return (a + b) / 2f + Vector3.up * heightOffset;
    }

    // 곡선 길이 추정
    float EstimateCurveLength(int steps)
    {
        float length = 0f;
        Vector3 prev = startPoint;

        // 구간을 쪼개어 각 길이 대략적으로 추정하여 더하기(미적과 같은 개념)
        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;
            Vector3 pos = CalculateCurvePos(t);

            length += Vector3.Distance(prev, pos);
            prev = pos;
        }

        return length;
    }

    // 현 시점에서의 커브 곡선 상 위치
    Vector3 CalculateCurvePos(float t)
    {
        Vector3 pos = Mathf.Pow(1 - t, 2) * startPoint +
               2 * (1 - t) * t * midPoint +
               Mathf.Pow(t, 2) * endPoint;

        return pos;
    }

    // 이동
    void Move()
    {
        // 속력이 3 unit/s이야.. 0.02s 당 한번씩 경신.
        // 둘을 곱하면 unit 단위가 나옴(1고정 업데이트 당 이동 거리)
        // 이걸 이어 붙이면 이동 거리
        // 그걸 totalLength로 나누면 0~1 사이 값으로 얼마나 진행되었는지 나옴..
        // 생각은 맞는 것 같은데 멀리 가면 그게 잘 안되네.. 속력으로 할 게 아니라 목적지까지 이동 시간으로 해야 정상 동작할 듯

        // 시간으로 바꿔서 생각해보자
        // moveTime = 3s 안에 목적지에 도달해야 한다면? 0.02s로 나눠주면 고정 업데이트 몇번만에 가야하는지 나옴 >> loopCountMax
        // 고정 루프 1번당 loopCount 쌓기 >> loopCount/loopCountMax = 진행도

        // 슬로우는 어떻게 해줘야 할까
        // loopCount를 float으로 바꿔버리면 되는 게 아닐까?
        // 평상시엔 1씩 더하다 슬로우 상태에서는 GameManager.Instance.slowRate 만큼씩 더하는걸루
        if (isSlowed)
            loopCount += GameManager.Instance.slowRate;
        else
            loopCount++;
        progressRate = loopCount / loopCountMax;
        progressRate = Mathf.Clamp01(progressRate); // 0~1 사이 값으로 제한하여 넘어가지 않도록

        Vector3 pos = Vector2.zero;
        switch (moveType)
        {
            case MoveType.Linear:
                pos = Vector2.Lerp(startPoint, endPoint, progressRate);
                break;

            case MoveType.Curve:
                pos = CalculateCurvePos(progressRate);
                break;
        }

        transform.position = pos;
        if (progressRate >= 1f)
            isMoving = false;
    }

    // 자전
    void SelfRotate()
    {
        if(isSelfRotating)
            // 쿼터니언의 곱은 순서 중요!
            // q1 * q2 = q2 회전 후 q1 회전하여 더하는 효과 >> 회전
            transform.rotation = rotationPerFixedFrame * transform.rotation;
    }


    // 슬로우/해제 관련 구현
    // 슬로우 중인지 판정하기 위한 프로퍼티(없으면 감속이 된 건지 아닌지 판정할 수 없기에)
    public bool isSlowed { get; set; } = false;
    // 감속 시작!
    public void StartSlow()
    {
        isSlowed = true;
        if(isSelfRotating_Default)
            rotationPerFixedFrame = Quaternion.Euler(Vector3.forward * rotationSpeed * GameManager.Instance.slowRate);
    }
    // 감속 끝!
    public void StopSlow()
    {
        isSlowed = false;
        if (isSelfRotating_Default)
            rotationPerFixedFrame = Quaternion.Euler(Vector3.forward * rotationSpeed);
    }
}
