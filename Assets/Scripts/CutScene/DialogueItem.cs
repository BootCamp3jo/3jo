using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueItem : MonoBehaviour
{
    public Image portrait;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI content;

    public void Init(Sprite portrait, string nameText)
    {
        if (portrait != null)
        {
            this.portrait.sprite = portrait;
        }
        if (nameText != null)
        {
            this.nameText.text = nameText;
        }
    }

    public void SetContent(string content)
    {
        this.content.text = content;
    }

}
