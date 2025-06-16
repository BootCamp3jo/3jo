using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string targetSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !string.IsNullOrEmpty(targetSceneName))
        {
            SceneTransitionController.Instance.StartSceneTransition(targetSceneName);
        }
    }
}
