using System.Collections.Generic;
using UnityEngine;

namespace npcDialogue
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string key;
        [TextArea]
        public List<string> texts; // 여러 줄 대사
    }

    [CreateAssetMenu(menuName = "Data/NPCDialogue")]
    public class DialogueData : ScriptableObject
    {
        public string npcId;
        public List<DialogueEntry> data;
    }
}
