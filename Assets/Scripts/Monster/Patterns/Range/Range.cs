using UnityEngine;

public class Range : MonoBehaviour
{
    public float atkReadyTime { get; set; }
    Vector3 scaleChangePerFrame;
    PatternRange pattern;
    bool isAtkStart;

    CircleMaskEffectController circleMask;
    float scaleSpeedMultiplier = 1f;

    private void Awake()
    {
        pattern = GetComponentInParent<PatternRange>(true);
        circleMask = CircleMaskEffectController.Instance;
    }

    private void OnEnable()
    {
        isAtkStart = false;
        transform.localScale = Vector3.zero;
        scaleChangePerFrame = Vector3.one / atkReadyTime;
    }

    private void Update()
    {
        AdjustSpeedByMask();

        transform.localScale += scaleChangePerFrame * Time.deltaTime * scaleSpeedMultiplier;

        if (transform.localScale.x >= 1f && !isAtkStart)
        {
            isAtkStart = true;
            transform.localScale = Vector3.one;

            pattern.rangeAttack.Attack();
            pattern.changeEffect.ChangeEffectAnime(pattern.changeEffect.impactHash);
        }
    }

    private void AdjustSpeedByMask()
    {
        if (circleMask == null || Camera.main == null) return;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 pos2D = new Vector2(viewportPos.x, viewportPos.y);
        float dist = Vector2.Distance(pos2D, circleMask.CurrentCenter);

        scaleSpeedMultiplier = (dist <= circleMask.CurrentRadius) ? 0.5f : 1f;
    }
}
