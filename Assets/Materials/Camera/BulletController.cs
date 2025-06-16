using UnityEngine;

// 오오 유빈 확인!
public class BulletController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float normalSpeed = 5f;
    public float slowSpeed = 2f;
    private float currentSpeed;

    private CircleMaskEffectController circleMask;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = normalSpeed;
        circleMask = CircleMaskEffectController.Instance;
    }

    private void FixedUpdate()
    {
        if (circleMask == null) return;

        // 현재 뷰포트 좌표로 자신의 위치 계산
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 pos2D = new Vector2(viewportPos.x, viewportPos.y);

        float distance = Vector2.Distance(pos2D, circleMask.CurrentCenter);

        // 반지름 이내면 속도 느리게, 아니면 원래 속도
        if (distance <= circleMask.CurrentRadius)
        {
            currentSpeed = slowSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }

        // 이동 방향은 Rigidbody2D velocity 벡터 방향 유지하면서 속도 조절
        Vector2 direction = rb.velocity.normalized;
        rb.velocity = direction * currentSpeed;
    }
}
