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
        [SerializeField] private float _charDelay = 0.05f;
        [SerializeField] private float _lineDelay = 1.0f;

        private Coroutine _displayCoroutine;
        private bool _isDialoguePlaying = false;

        public bool IsDialoguePlaying => _isDialoguePlaying;

        private void Awake()
        {
            _dialogueUI.SetActive(false);
        }

        public void ShowLines(List<string> lines)
        {
            if (_isDialoguePlaying)
                return;

            if (_displayCoroutine != null)
            {
                StopCoroutine(_displayCoroutine);
                _displayCoroutine = null;
            }

            _displayCoroutine = StartCoroutine(ShowLinesCoroutine(lines));
        }

        private IEnumerator ShowLinesCoroutine(List<string> lines)
        {
            _isDialoguePlaying = true;
            _dialogueText.text = "";
            _dialogueUI.SetActive(true);

            foreach (string line in lines)
            {
                yield return StartCoroutine(TypeText(line));
                yield return new WaitForSeconds(_lineDelay);
            }

            _dialogueUI.SetActive(false);
            _isDialoguePlaying = false;
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
