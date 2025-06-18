using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace npcDialogue
{
    enum DialogueKey
    {
        Near,
        Click,
    }

    public class DialogueNpc : BaseNpc
    {
        [SerializeField] private DialogueData _dialogueData;
        private NpcDialogueController _dialogueController;

        private void Awake()
        {
            _dialogueController = GetComponent<NpcDialogueController>();
        }

        public override void InteractiveNPC()
        {
            base.InteractiveNPC();
            ShowTextBox(DialogueKey.Click.ToString());
        }

        public void ShowTextBox(string key)
        {
            var lines = GetDialogueLinesByKey(key);
            if (lines != null && lines.Count > 0)
            {
                _dialogueController.ShowLines(lines);
            }
        }

        List<string> GetDialogueLinesByKey(string key)
        {
            return _dialogueData.data.FirstOrDefault(d => d.key == key)?.texts;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerOnly"))
            {
                if (!_dialogueController.IsDialoguePlaying)
                {
                    ShowTextBox(DialogueKey.Near.ToString());
                }
            }
        }
    }
}
