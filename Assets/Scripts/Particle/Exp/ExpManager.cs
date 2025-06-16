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

    private WaitForSeconds waitTime = new WaitForSeconds(0.1f);
    private AudioManager audioManager;
    private void Awake()
    {
        audioManager = AudioManager.instance;
    }
    public void SpawnExp(Vector3 worldPosition, int count)
    {
        StartCoroutine(SpawnExpCoroutine(worldPosition, count));
    }

    IEnumerator SpawnExpCoroutine(Vector3 worldPosition, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = worldPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0, 0);
            GameObject orb = Instantiate(xpOrbPrefab, spawnPos, Quaternion.identity);
            orb.GetComponent<ExpOrb>().Init(uiImage, uiTarget, () => OnXPCollected(10));

            yield return waitTime; // ер аж╠Б
        }
    }

    void OnXPCollected(int amount)
    {
        audioManager.PlaySFX(SFXType.AddExp,0.7f,1.0f);
        OnExpGained?.Invoke(amount);
    }
}
