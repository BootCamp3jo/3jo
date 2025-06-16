using UnityEngine;

public class CircleMaskEffectController : MonoBehaviour
{
    public static CircleMaskEffectController Instance;

    [Range(0f, 1f)] public float maxRadius = 0.5f;
    public float expandDuration = 1f;
    public float holdDuration = 1f;
    public float shrinkDuration = 1f;

    public Transform effectCenter; // ✅ 중심이 될 Transform (빈 오브젝트 지정 가능)

    public float CurrentRadius { get; private set; } = 0f;
    public Vector2 CurrentCenter { get; private set; } = new Vector2(0.5f, 0.5f); // ✅ 쉐이더로 보낼 중심

    private enum State { Idle, Expanding, Holding, Shrinking }
    private State currentState = State.Idle;
    private float timer = 0f;

    // ISlowable의 입출입을 판정하기가 쉽지 않음.. 트리거로 해볼게요
    CircleCollider2D trigger;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        TryGetComponent(out trigger);
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Expanding:
                timer += Time.deltaTime;
                CurrentRadius = Mathf.Lerp(0f, maxRadius, timer / expandDuration);
                trigger.radius = CurrentRadius;
                if (timer >= expandDuration)
                {
                    currentState = State.Holding;
                    timer = 0f;
                }
                break;

            case State.Holding:
                timer += Time.deltaTime;
                CurrentRadius = maxRadius;
                trigger.radius = CurrentRadius;
                if (timer >= holdDuration)
                {
                    currentState = State.Shrinking;
                    timer = 0f;
                }
                break;

            case State.Shrinking:
                timer += Time.deltaTime;
                CurrentRadius = Mathf.Lerp(maxRadius, 0f, timer / shrinkDuration);
                trigger.radius = CurrentRadius;
                if (timer >= shrinkDuration)
                {
                    currentState = State.Idle;
                    timer = 0f;
                    CurrentRadius = 0f;
                }
                break;

            case State.Idle:
                CurrentRadius = 0f;
                trigger.enabled = false;
                break;
        }
    }

    public Vector3 pivotOffset = Vector3.zero; // 피벗 변경분 만큼 넣어줄 오프셋

public void TriggerEffect()
{
    if (effectCenter != null)
    {
        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 worldPos = effectCenter.position + pivotOffset;
            Debug.Log($"Effect center worldPos: {worldPos}");

            Vector3 viewportPos = cam.WorldToViewportPoint(worldPos);
            Debug.Log($"Effect center viewportPos: {viewportPos}");

            CurrentCenter = trigger.offset = new Vector2(viewportPos.x, viewportPos.y);
            trigger.enabled = true;
        }
    }

    currentState = State.Expanding;
    timer = 0f;
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ISlowAble slowableObj))
        {
            if (!slowableObj.isSlowed)
            {
                slowableObj.StartSlow();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ISlowAble slowableObj))
        {
            if (slowableObj.isSlowed)
            {
                slowableObj.StopSlow();
            }
        }
    }

}