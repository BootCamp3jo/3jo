using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CutSceneManager : MonoSingleton<CutSceneManager>
{
    public CutSceneData streamCutSceneData; // 컷씬 데이터 받아오기
    public DialoguePanelManager dialogueManager;
    private Transform mainCamera;
    private Vector3 cameraOriginPos;
    private AudioSource audio;

    // 컷씬 코루틴
    private Coroutine cutSceneCorWrap;
    private bool _selectActionTriggered = false; // 선택 지문 대기 bool
    private int selectedPoint;
    private int selectedNum;

    private void Start()
    {
        if (audio == null) { audio = GetComponent<AudioSource>(); }
        if (dialogueManager == null) { dialogueManager = FindAnyObjectByType<DialoguePanelManager>(); }

        StartCutScene(streamCutSceneData);
    }

    public void StartCutScene(CutSceneData cutSceneData) // 사용법 : CutSceneManager.Instance.StartCutScene(컷씬 데이터);
    {
        this.streamCutSceneData = cutSceneData;
        if (mainCamera == null)
        {
            mainCamera = Camera.main.transform;
        }
        cameraOriginPos = mainCamera.transform.position;

        cutSceneCorWrap = StartCoroutine(CutSceneCor());
    }

    IEnumerator CutSceneCor()
    {
        foreach (CutData data in streamCutSceneData.cutList)
        {
            if (data.isDialogue == false) // 카메라 컷씬
            {
                dialogueManager.gameObject.SetActive(false);
                if (data.cameraSceneData.cutSound != null)
                {
                    audio.PlayOneShot(data.cameraSceneData.cutSound);
                }
                mainCamera.transform.position = data.cameraSceneData.cameraWayPointList[0]; // 카메라 위치 초기화

                float timer = 0f;
                float segmentTime = data.cameraSceneData.cutTime / (data.cameraSceneData.cameraWayPointList.Count - 1);

                for (int i = 0; i < data.cameraSceneData.cameraWayPointList.Count - 1; i++) // 카메라 이동
                {
                    Vector3 startPos = data.cameraSceneData.cameraWayPointList[i];
                    Vector3 endPos = data.cameraSceneData.cameraWayPointList[i + 1];

                    timer = 0f;
                    while (timer < segmentTime)
                    {
                        timer += Time.deltaTime;
                        float t = Mathf.Clamp01(timer / segmentTime);
                        mainCamera.transform.position = Vector3.Lerp(startPos, endPos, t);
                        yield return null;
                    }
                }

                yield return new WaitForSeconds(data.cameraSceneData.delayTime);
                mainCamera.transform.position = cameraOriginPos; // 카메라 다시 원래대로
            }
            else // 다이얼로그 컷씬
            {
                selectedPoint = -1;
                dialogueManager.gameObject.SetActive(true);
                dialogueManager.Init();
                for (int i = 0; i < data.dialogueSceneData.dialogueList.Count; i++)
                {
                    if (selectedPoint != -1 &&
                    data.dialogueSceneData.dialogueList[selectedPoint].subDialogueDataList.Any(x => x.nextDialogue == i) &&
                    i != selectedNum)
                    {

                    }
                    else
                    {
                        if (data.dialogueSceneData.dialogueList[i].chatSound != null)
                        {
                            audio.PlayOneShot(data.dialogueSceneData.dialogueList[i].chatSound);
                        }

                        yield return dialogueManager.ShowDialogue(data.dialogueSceneData.dialogueList[i], () =>
                        {
                        });

                        if (data.dialogueSceneData.dialogueList[i].isSelectDialogue == true) // 선택지문 있을 시
                        {
                            _selectActionTriggered = false;
                            dialogueManager.ShowSelect();

                            yield return new WaitUntil(() => _selectActionTriggered); // 선택 대기
                            _selectActionTriggered = false;

                            if (selectedNum != -1)
                            {
                                selectedPoint = i;
                                i = selectedNum - 1;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }

                dialogueManager.Init();
                dialogueManager.gameObject.SetActive(false);
            }
        }
        yield return null;

        if (streamCutSceneData.nextScene != null && streamCutSceneData.nextScene != "")
        {
            //씬 로드
        }
    }

    public void OnSelectDialogue(int num)
    {
        selectedNum = num;
        _selectActionTriggered = true;
    }
}
