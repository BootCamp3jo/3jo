using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{
    public static SceneTransitionController Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void StartSceneTransition(string targetScene)
    {
        StartCoroutine(FadeAndLoadScene(targetScene));
    }

    private IEnumerator FadeAndLoadScene(string targetScene)
    {
        Time.timeScale = 0f;
        yield return StartCoroutine(Fade(0f, 1f)); // 검은 화면으로 덮기

        Time.timeScale = 1f; // 씬 이동은 TimeScale 1에서 진행돼야 함
        yield return SceneManager.LoadSceneAsync(targetScene);

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.1f); // 짧은 대기

        yield return StartCoroutine(Fade(1f, 0f)); // 검은 화면 걷기
        Time.timeScale = 1f;
    }

    private IEnumerator Fade(float from, float to)
    {
        float time = 0f;
        Color color = fadeImage.color;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(from, to, t);
            fadeImage.color = color;
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        color.a = to;
        fadeImage.color = color;
    }
}
