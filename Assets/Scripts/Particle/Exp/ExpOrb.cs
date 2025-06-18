using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ExpOrb : MonoBehaviour
{
    public float moveDuration = 1.5f;
    private System.Action onReachTarget;

    private RectTransform expBarRT;
    private Image expBarImage;
    private Camera uiCamera;
    private Vector3 controlPoint;
    private Vector3 startPos;
    private float elapsed = 0f;

    private bool isMoving = false;
    public int orbPerExp = 100;
    public void Init(Image expBarImage, RectTransform expBarRT, System.Action onReached)
    {
        this.expBarImage = expBarImage;
        this.expBarRT = expBarRT;
        this.onReachTarget = onReached;

        Canvas canvas = expBarRT.GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;

        startPos = transform.position;
        isMoving = true;
        elapsed = 0f;

        // 초기 경로 계산용 제어점 설정
        Vector3 end = GetTargetWorldPosition();
        Vector3 direction = end - startPos;
        Vector3 perpendicular = Vector3.Cross(direction.normalized, Vector3.forward);
        float curveStrength = Random.Range(-2.0f, 2.0f);
        controlPoint = startPos + direction * 0.5f + perpendicular * curveStrength;
    }

    void Update()
    {
        if (!isMoving) return;

        elapsed += Time.deltaTime;
        float t = Mathf.Clamp01(elapsed / moveDuration);

        Vector3 end = GetTargetWorldPosition();

        // 베지어 곡선 계산 (Quadratic Bezier)
        Vector3 m1 = Vector3.Lerp(startPos, controlPoint, t);
        Vector3 m2 = Vector3.Lerp(controlPoint, end, t);
        Vector3 pos = Vector3.Lerp(m1, m2, t);

        transform.position = pos;

        if (t >= 1f)
        {
            isMoving = false;
            onReachTarget?.Invoke();
            Destroy(gameObject);
        }
    }

    private Vector3 GetTargetWorldPosition()
    {
        float fill = expBarImage.fillAmount;
        Vector2 barSize = expBarRT.rect.size;

        Vector2 localEndPos = new Vector2(
            barSize.x * (fill - expBarRT.pivot.x),
            barSize.y * (0.5f - expBarRT.pivot.y)
        );

        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, expBarRT.TransformPoint(localEndPos));

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;

        return worldPos;
    }
}
