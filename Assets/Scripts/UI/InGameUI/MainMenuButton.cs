using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private string nextScene = "MainScene_Art";
    public void OnClick()
    {
        Time.timeScale = 1.0f;
        SceneTransitionController.Instance.StartSceneTransition(nextScene);
    }
}
