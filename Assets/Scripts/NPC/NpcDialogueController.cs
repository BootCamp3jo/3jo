using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace npcDialogue
{
    public class NpcDialogueController : MonoBehaviour
    {
        [SerializeField] private GameObject _dialogueUI;
        [SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private float _charDelay = 0.05f; // 글자당 딜레이
        [SerializeField] private float _lineDelay = 1.0f;  // 줄 간 딜레이

        private Coroutine _displayCoroutine;

        private void Awake()
        {
            _dialogueUI.SetActive(false);
        }

        public void ShowLines(List<string> lines)
        {
            _dialogueUI.SetActive(true);

            if (_displayCoroutine != null)
                StopCoroutine(_displayCoroutine);

            _displayCoroutine = StartCoroutine(ShowLinesCoroutine(lines));
        }

        private IEnumerator ShowLinesCoroutine(List<string> lines)
        {
            foreach (string line in lines)
            {
                yield return StartCoroutine(TypeText(line));
                yield return new WaitForSeconds(_lineDelay);
            }

            _dialogueUI.SetActive(false);
            _displayCoroutine = null;
        }

        private IEnumerator TypeText(string text)
        {
            _dialogueText.text = "";

            foreach (char c in text)
            {
                _dialogueText.text += c;
                yield return new WaitForSeconds(_charDelay);
            }
        }
    }
}
