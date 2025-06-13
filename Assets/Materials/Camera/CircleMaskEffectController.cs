using UnityEngine;

public class CircleMaskEffectController : MonoBehaviour
{
    public static CircleMaskEffectController Instance;

    [Range(0f, 1f)] public float maxRadius = 0.5f;
    public float expandDuration = 1f;
    public float holdDuration = 1f;
    public float shrinkDuration = 1f;

    public float CurrentRadius { get; private set; } = 0f;

    private enum State { Idle, Expanding, Holding, Shrinking }
    private State currentState = State.Idle;
    private float timer = 0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentState = State.Expanding;
        CurrentRadius = 0f;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Expanding:
                timer += Time.deltaTime;
                CurrentRadius = Mathf.Lerp(0f, maxRadius, timer / expandDuration);
                if (timer >= expandDuration)
                {
                    currentState = State.Holding;
                    timer = 0f;
                }
                break;

            case State.Holding:
                timer += Time.deltaTime;
                CurrentRadius = maxRadius;
                if (timer >= holdDuration)
                {
                    currentState = State.Shrinking;
                    timer = 0f;
                }
                break;

            case State.Shrinking:
                timer += Time.deltaTime;
                CurrentRadius = Mathf.Lerp(maxRadius, 0f, timer / shrinkDuration);
                if (timer >= shrinkDuration)
                {
                    currentState = State.Idle;
                    timer = 0f;
                    CurrentRadius = 0f;
                }
                break;

            case State.Idle:
                CurrentRadius = 0f;
                break;
        }
    }

    // 이 함수로 효과를 시작할 수 있음
    public void TriggerEffect()
    {
        currentState = State.Expanding;
        timer = 0f;
    }
}
