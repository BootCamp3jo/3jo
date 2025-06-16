using UnityEngine;

public class ChangeEffect : MonoBehaviour
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
}
