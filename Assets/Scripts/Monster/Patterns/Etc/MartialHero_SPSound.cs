using UnityEngine;

public class MartialHero_SPSound : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlaySFX(SFXType.MartialHero_SPAttack);
    }
}
