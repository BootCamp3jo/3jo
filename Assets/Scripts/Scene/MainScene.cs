using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{

    private void OnEnable()
    {
        AudioManager.instance.PlayBGM(BGMType.Main);
    }

}
