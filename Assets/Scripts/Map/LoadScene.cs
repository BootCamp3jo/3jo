using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private string targetSceneName;

    public void OnLoadScene()
    {
        SceneTransitionController.Instance.StartSceneTransition(targetSceneName);
    }

}
