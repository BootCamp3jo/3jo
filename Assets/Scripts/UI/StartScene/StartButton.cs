using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private void Start()
    {
        if(this.gameObject.TryGetComponent<Button>(out Button button))
        {
            button.onClick.AddListener(() => SceneManager.LoadSceneAsync("SelectSaveScene"));
        }
    }
}
