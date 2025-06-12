using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ExpOrb : MonoBehaviour
{
    public float moveDuration = 1.5f;
    private System.Action onReachTarget;

    public void Init(Image expBarImage, RectTransform expBarRT, System.Action onReached)
    {
        onReachTarget = onReached;

        float fill = expBarImage.fillAmount;
        Vector2 barSize = expBarRT.rect.size;

        // pivot 보정 적용
        Vector2 localEndPos = new Vector2(
            barSize.x * (fill - expBarRT.pivot.x),
            barSize.y * (0.5f - expBarRT.pivot.y)
        );

        Canvas canvas = expBarRT.GetComponentInParent<Canvas>();
        Camera uiCamera = canvas.worldCamera;

        // local -> screen -> world
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(uiCamera, expBarRT.TransformPoint(localEndPos));
        Vector3 end = Camera.main.ScreenToWorldPoint(screenPos);
        end.z = 0; // z 보정

        Vector3 start = transform.position;
        Vector3 direction = end - start;
        Vector3 perpendicular = Vector3.Cross(direction.normalized, Vector3.forward);
        float curveStrength = Random.Range(-2.0f, 2.0f);
        Vector3 control = start + direction * 0.5f + perpendicular * curveStrength;

        Vector3[] path = new Vector3[] { start, control, end };

        transform.DOPath(path, moveDuration, PathType.CatmullRom)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => {
                onReachTarget?.Invoke();
                Destroy(gameObject);
            });
    }
}
