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


    }
    private void Start()
    {
        audioManager = AudioManager.instance;
        playerStatHandler = PlayerManager.Instance.playerStatHandler;
    }
    public void SpawnExp(Vector3 worldPosition, int count, float delayAfter = 0f, System.Action onComplete = null)
    {
        StartCoroutine(SpawnExpCoroutine(worldPosition, count, delayAfter, onComplete));
    }

    IEnumerator SpawnExpCoroutine(Vector3 worldPosition, int count, float delayAfter, System.Action onComplete)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = worldPosition + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0, 0);
            GameObject orb = Instantiate(expOrbPrefab, spawnPos, Quaternion.identity);
            var expPerOrb = orb.GetComponent<ExpOrb>().orbPerExp;
            orb.GetComponent<ExpOrb>().Init(uiImage, uiTarget, () => OnXPCollected(expPerOrb));

            yield return waitTime;
        }

        if (delayAfter > 0f)
            yield return new WaitForSeconds(delayAfter);

        onComplete?.Invoke(); // 완료 후 콜백 실행
    }
    void OnXPCollected(int amount)
    {
        audioManager.PlaySFX(SFXType.AddExp,0.7f,1.0f);

        playerStatHandler.AddExperience(amount);
        //OnExpGained?.Invoke(amount);
    }
}
