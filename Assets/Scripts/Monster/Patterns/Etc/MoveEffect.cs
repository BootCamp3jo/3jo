using UnityEngine;

public enum MoveType
{
    Linear,
    Curve,
}

// 공격 이펙트가 이동할 때 사용(탄환, 장판 범위 표시 후 날아가는 투사체 등)
public class MoveEffect : MonoBehaviour
{
    public MoveType moveType;
    // 이동 속력
    public float moveSpeed = 3f;
    // 곡선일 때 중점의 높이
    public float heigthOfPathCurve = 3;

    Vector3 startPoint, midPoint, endPoint;

    float t = 0f;
    float totalLength;
    bool isMoving = false;


    void OnEnable()
    {
        startPoint = transform.parent.position;
        endPoint = PlayerManager.Instance.playerPrefab.transform.position;

        switch (moveType)
        {
            case MoveType.Linear:
                break;
            case MoveType.Curve:
                MidPoint(startPoint, endPoint, heigthOfPathCurve);
                totalLength = EstimateCurveLength(20);
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;

        t += (moveSpeed * Time.fixedDeltaTime) / totalLength;
        t = Mathf.Clamp01(t);

        Vector3 pos = CalculateCurvePos(t);

        transform.position = pos;

        if (t >= 1f)
            isMoving = false;
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

    Vector3 CalculateCurvePos(float t)
    {
        Vector3 pos = Mathf.Pow(1 - t, 2) * startPoint +
               2 * (1 - t) * t * midPoint +
               Mathf.Pow(t, 2) * endPoint;

        return pos;
    }
}
