using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningScene : MonoBehaviour
{
    public CutSceneData openingData; // 컷씬 데이터

    void Start()
    {
        if (CutSceneManager.Instance != null)
        {
            CutSceneManager.Instance.StartCutScene(openingData);
        }
    }

}
