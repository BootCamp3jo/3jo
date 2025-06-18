using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClick()
    {
        PlayerData playerData = PlayerManager.Instance.playerData;
        PlayerManager.Instance.playerStatHandler.Heal(playerData.MaxHealth - playerData.CurrentHealth);
        DataManager.Instance.ClearCurSceneBundle();
        DataManager.Instance.DontSaveCurSceneBundle();
    }
}
