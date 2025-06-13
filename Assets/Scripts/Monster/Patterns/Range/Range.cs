using UnityEngine;

public class Range : MonoBehaviour
{
    public float atkReadyTime;
    Vector3 scaleChangePerFrame;
    bool isAtkPerformed;

    private void OnEnable()
    {
        // 범위 표시 초기화
        transform.localScale = Vector3.zero;
        // 공격 준비 초기화
        isAtkPerformed = false;
        // 프레임 당 스케일 변화량 초기화(atkReadyTime의 변동이 있는 기술 대비)
        scaleChangePerFrame = Vector3.one / atkReadyTime;
    }

    private void Update()
    {
        // 공격 수행 다음 프레임에 여기로 들어와서
        if (isAtkPerformed)
            // 공격 범위 오브젝트 비활성화
            transform.parent.gameObject.SetActive(false);

        // 공격 범위에 맞게 점점 커지는 경고 영역 
        transform.localScale += scaleChangePerFrame * Time.deltaTime;
        // 공격 범위가 다 채워졌다면
        if (transform.localScale.x >= 1)
        {
            // 범위 밖으로 벗어나지 않게
            transform.localScale = Vector3.one;
            // 공격!

            // 공격이 수행되었음을 알림
            isAtkPerformed = true;
        }
    }
}
