using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DialogueSelectItem : MonoBehaviour
{
    private int nextCode;
    public TextMeshProUGUI content;

    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(this.OnSelect);
    }

    public void Init(string content, int nextCode)
    {
        this.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        this.transform.DOScale(1, 0.2f);
        this.content.text = content;
        this.nextCode = nextCode;
    }

    public void OnSelect()
    {
        CutSceneManager.Instance.OnSelectDialogue(this.nextCode);
    }
}
