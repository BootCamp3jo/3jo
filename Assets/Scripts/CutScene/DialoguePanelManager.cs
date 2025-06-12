using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePanelManager : MonoBehaviour
{
    public Transform leftChat;
    public Transform explainChat;
    public Transform playerChat;
    private DialogueData data;

    public void SetDialogue(DialogueData data)
    {
        this.data = data;
        ShowDialogue();
    }

    public void ShowDialogue()
    {

    }

}
