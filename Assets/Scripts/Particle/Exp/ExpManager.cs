using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ExpManager : MonoBehaviour
{
    public GameObject xpOrbPrefab;
    public RectTransform uiTarget;
    public Image uiImage;

    public static event Action<int> OnExpGained; 

    public void SpawnXP(Vector3 worldPosition, int count)
    {
        StartCoroutine(SpawnXPCoroutine(worldPosition, count));
    }

    IEnumerator SpawnXPCoroutine(Vector3 worldPosition, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = worldPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0, 0);
            GameObject orb = Instantiate(xpOrbPrefab, spawnPos, Quaternion.identity);
            orb.GetComponent<ExpOrb>().Init(uiImage, uiTarget, () => OnXPCollected(10));

            yield return new WaitForSeconds(0.1f); // ер аж╠Б
        }
    }

    void OnXPCollected(int amount)
    {
        OnExpGained?.Invoke(amount);
    }
}
