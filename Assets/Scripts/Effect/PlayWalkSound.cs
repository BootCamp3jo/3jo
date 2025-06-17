using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayWalkSound : MonoBehaviour
{
    public void PlayWalkSFX()
    {
        AudioManager.instance.PlaySFX(SFXType.WalkGrass);
    }
}
