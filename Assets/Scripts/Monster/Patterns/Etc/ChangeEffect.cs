using UnityEngine;

public class ChangeEffect : MonoBehaviour, ISlowAble
{
    string impact = "Impact";
    public int impactHash { private set; get; }
    Animator animator;

    private void Awake()
    {
        TryGetComponent(out animator);
        impactHash = Animator.StringToHash(impact);
    }

    public void ChangeEffectAnime(int hash)
    {
        animator.SetTrigger(hash);
    }


    // 슬로우/해제 관련 구현
    // 슬로우 중인지 판정하기 위한 프로퍼티(없으면 감속이 된 건지 아닌지 판정할 수 없기에)
    public bool isSlowed { get; set; } = false;
    // 감속 시작!
    public void StartSlow()
    {
        isSlowed = true;
        float slowRateGlobal = GameManager.Instance.slowRate;
        // 애니메이터 전체 속도 감속
        animator.speed = slowRateGlobal;
    }
    // 감속 끝!
    public void StopSlow()
    {
        isSlowed = false;
        // 애니메이터 전체 속도 원복
        animator.speed = 1;
    }
}
