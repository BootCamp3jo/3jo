using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DialoguePanelManager : MonoBehaviour
{
    public List<DialogueItem> chatItems;
    public Transform selectPanel;
    public GameObject selectItemPrefab;
    private List<GameObject> selectItems;
    private DialogueData data;
    public AudioClip textBeepSound;

    public void Init()
    {
        selectPanel.gameObject.SetActive(false);
        for (var i = 0; i < chatItems.Count; i++)
        {
            chatItems[i].gameObject.SetActive(false);
        }

        if (selectItems?.Count > 0)
        {
            for (var i = 0; i < selectItems.Count; i++)
            {
                Destroy(selectItems[i]);
            }
            selectItems = new List<GameObject>();
        }
        if (selectItems == null)
        {
            selectItems = new List<GameObject>();
        }
    }

    public IEnumerator ShowDialogue(DialogueData data, System.Action onCompleteCallback = null)
    {
        this.data = data;
        // 세팅
        selectPanel.gameObject.SetActive(false);
        chatItems[(int)data.dialogueType].gameObject.SetActive(true);
        chatItems[(int)data.dialogueType].transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        chatItems[(int)data.dialogueType].transform.DOScale(1, 0.2f);
        chatItems[(int)data.dialogueType].GetComponent<CanvasGroup>().alpha = 1f;

        if (data.dialogueType != DialogueType.Center) // 센터가 아닐 때
        {
            chatItems[(int)data.dialogueType].Init(data.portrait, data.title);
        }
        else
        {
            for (var i = 0; i < chatItems.Count; i++)
            {
                if (i != (int)DialogueType.Center)
                {
                    chatItems[i].gameObject.SetActive(false);
                }
            }
        }

        // 글자 표시 반복문
        float delayPerCharacter = 1f / data.chatTime;
        string chatString = "";
        for (int i = 0; i < data.content.Length; i++)
        {
            chatString += data.content[i]; // 한 글자씩 추가
            chatItems[(int)data.dialogueType].SetContent(chatString);
            // 비프 사운드 재생
            CutSceneManager.Instance.audio.PlayOneShot(textBeepSound);

            yield return new WaitForSeconds(delayPerCharacter); // 계산된 시간만큼 대기
        }

        yield return new WaitForSeconds(data.delayTime);

        if (data.dialogueType != DialogueType.Center)
        {
            switch (data.lastState)
            {
                case DialogueLastState.Shadow: // shadow(어둡게)
                    {
                        chatItems[(int)data.dialogueType].GetComponent<CanvasGroup>().alpha = 0.5f;
                        break;
                    }
                case DialogueLastState.None: // hide(안 보임)
                    {
                        chatItems[(int)data.dialogueType].GetComponent<CanvasGroup>().alpha = 0f;
                        break;
                    }
            }
        }
        else
        {
            chatItems[(int)data.dialogueType].gameObject.SetActive(false);
        }

        yield return null;
        onCompleteCallback?.Invoke();
    }

    public void ShowSelect()
    {
        Init();
        selectPanel.gameObject.SetActive(true);
        for (int i = 0; i < data.subDialogueDataList.Count; i++)
        {
            GameObject @Go = Instantiate(selectItemPrefab, selectPanel, false);
            selectItems.Add(@Go);
            @Go.GetComponent<DialogueSelectItem>().Init(data.subDialogueDataList[i].content, data.subDialogueDataList[i].nextDialogue);
        }
    }

}
