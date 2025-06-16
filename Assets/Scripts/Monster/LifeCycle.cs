using UnityEngine;

public class LifeCycle : MonoBehaviour, ISlowAble
{
    // 지속 시간
    public float lifeTime { get; set; }

    // 지속 시간 빼주기! >> 슬로우가 되면 줄어드는 속도도 감소
    public void Update()
    {
        if(isSlowed)
            lifeTime -= GameManager.Instance.slowRate * Time.deltaTime;
        else
            lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
            LifeEnd();
    }

    public virtual void LifeEnd()
    {
        gameObject.SetActive(false);
    }

    // 슬로우/해제 관련 구현
    // 슬로우 중인지 판정하기 위한 프로퍼티(없으면 감속이 된 건지 아닌지 판정할 수 없기에)
    public bool isSlowed { get; set; } = false;
    // 감속 시작!
    public void StartSlow()
    {
        isSlowed = true;
    }
    // 감속 끝!
    public void StopSlow()
    {
        isSlowed = false;
    }
}
