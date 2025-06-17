using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySkillSound : MonoBehaviour
{
    [SerializeField] SFXType type;

    public void PlaySkillSfx()
    {
        AudioManager.instance.PlaySFX(type);
    }

}
