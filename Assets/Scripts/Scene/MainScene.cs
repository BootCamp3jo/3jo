using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{

    private void Start()
    {
        AudioManager.instance.PlayBGM(BGMType.Main);
    }

}
