using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetAndStartSceneButton : MonoBehaviour
{
    [SerializeField]
    private string nextScene;
    private void Start()
    {
        if (gameObject.TryGetComponent<Button>(out Button button))
        {
            button.onClick.AddListener(() =>
            {
                DataManager.Instance.gameContext.ResetSave();
                SceneManager.LoadScene(nextScene);
            });
        }
    }
}
