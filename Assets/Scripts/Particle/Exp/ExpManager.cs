using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ExpManager : MonoBehaviour
{
    public static ExpManager instance { get; private set; }
    public GameObject expOrbPrefab;
    public RectTransform uiTarget;
    public Image uiImage;
    public static event Action<int> OnExpGained;

    private WaitForSeconds waitTime = new WaitForSeconds(0.1f);
    private AudioManager audioManager;
    private PlayerStatHandler playerStatHandler;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        audioManager = AudioManager.instance;
    }
    private void Start()
    {
         playerStatHandler = PlayerManager.Instance.playerStatHandler;
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
            GameObject orb = Instantiate(expOrbPrefab, spawnPos, Quaternion.identity);
            orb.GetComponent<ExpOrb>().Init(uiImage, uiTarget, () => OnXPCollected(10));

            yield return waitTime; // 텀 주기
        }
    }

    void OnXPCollected(int amount)
    {
        audioManager.PlaySFX(SFXType.AddExp,0.7f,1.0f);

        playerStatHandler.AddExperience(amount);
        //OnExpGained?.Invoke(amount);
    }
}
