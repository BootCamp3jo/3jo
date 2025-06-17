using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.instance.PlayBGM(BGMType.Battle);
    }
}
