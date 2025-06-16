using UnityEngine;

// 스스로 회전하는 패턴 이펙트에 붙이기 위함
// 애니메이션에 같이 쓰기에는 반복되면서 회전각이 초기화.. 방법이 있기는 할 듯한데 시간이 없으니 스크립트로 빠르게 처리
public class SelfRotate : MonoBehaviour
{
    // 회전 속도
    public float rotationSpeed;
    // 회전 지속 시간
    bool stopRotate;
    Quaternion rotationPerFixedFrame;

    private void OnEnable()
    {
        stopRotate = false;
    }

    private void Start()
    {
        // Time.fixedDeltaTime 값은 고정(보통 0.02초)이기에 매번 같은 값으로 회전 >> 곱하지 않고 그냥 rotationSpeed만으로 조정
        rotationPerFixedFrame = Quaternion.Euler(Vector3.forward * rotationSpeed);
    }

    private void FixedUpdate()
    {
        if (stopRotate) return;

        // 쿼터니언의 곱은 순서 중요!
        // q1 * q2 = q2 회전 후 q1 회전하여 더하는 효과 >> 회전
        transform.rotation =  rotationPerFixedFrame * transform.rotation;
    }

    public void EndRotate()
    {
        stopRotate = true;
    }
}
