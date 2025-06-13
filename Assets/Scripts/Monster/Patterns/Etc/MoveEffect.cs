using UnityEngine;
using DG.Tweening;

// 공격 이펙트가 이동할 때 사용(탄환, 장판 범위 표시 후 날아가는 투사체 등)
public class MoveEffect : MonoBehaviour
{
    // 이동 타입 !!! >> 튜터님께 보여준 후 타입별로 작업하여 다른 패턴 기반 만들기
    public PathType pathType;
    // 이동 시간
    public float duration = 2f;
    // 곡선일 때 중점의 높이
    public float heigthOfPathCurve = 3;

    Vector3 startPoint, midPoint, endPoint;


    void OnEnable()
    {
        switch (pathType)
        {
            case PathType.Linear:
                break;
            case PathType.CatmullRom:
                Vector3[] path = new Vector3[] { startPoint, midPoint, endPoint };
                transform.position = startPoint;
                // DOPath : 여러 점을 따라 곡선 경로로 이동
                transform.DOPath(path, duration, PathType.CatmullRom)
                         .SetEase(Ease.InOutSine)
                         .OnComplete(() => Debug.Log("착탄 이펙트 호출 후 오브젝트 풀에 다시 넣기"));
                break;
            case PathType.CubicBezier:
                break;
            default:
                break;
        }
    }

    // 이번 패턴 호출 때 시작점과 끝 점을 지정 >> 경로 생성
    public void SetPath(PathType pathType, Vector3 start, Vector3 end)
    {
        switch (pathType)
        {
            case PathType.Linear:
                break;
            case PathType.CatmullRom:
                startPoint = start;
                endPoint = end;
                midPoint = MidPoint(start, end);
                break;
            case PathType.CubicBezier:
                break;
            default:
                break;
        }
    }

    // 곡선의 중점
    Vector3 MidPoint(Vector3 a, Vector3 b, float heightOffset = 0f)
    {
        // 시작과 끝의 중간지점에서 y축 방향으로 원하는 높이만큼 올라간 지점
        return (a + b) / 2f + Vector3.up * heightOffset;
    }
}
