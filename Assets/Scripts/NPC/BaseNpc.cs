using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace npcDialogue
{
    public class BaseNpc : MonoBehaviour
    {


        // 상호작용, npc마다 상호작용은 달라질 수 있음.
        public virtual void InteractiveNPC()
        {
            StartCoroutine(Shake(0.2f, 0.1f));
        }
        IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 originalPos = transform.localPosition;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float offsetX = Random.Range(-1f, 1f) * magnitude;
                float offsetY = Random.Range(-1f, 1f) * magnitude;

                transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = originalPos;
        }
    }

}
