using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoSingleton<CutSceneManager>
{
    public CutSceneData streamCutSceneData; // 컷씬 데이터 받아오기
    public DialoguePanelManager dialoguePanel;
    private Transform mainCamera;
    private Vector3 cameraOriginPos;
    private AudioSource audio;

    private void Start()
    {
        if (audio == null) { audio = GetComponent<AudioSource>(); }
        if (dialoguePanel == null) { dialoguePanel = FindAnyObjectByType<DialoguePanelManager>(); }
    }

    public void StartCutScene(CutSceneData cutSceneData)
    {
        this.streamCutSceneData = cutSceneData;
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }
        cameraOriginPos = mainCamera.transform.position;
    }

}
