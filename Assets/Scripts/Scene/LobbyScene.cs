using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour
{
    private void OnEnable()
    {
        AudioManager.instance.PlayBGM(BGMType.Lobby);
    }
}
