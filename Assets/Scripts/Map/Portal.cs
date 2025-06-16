using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    public bool IsPlayerInPortal = false;
    [SerializeField] public GameObject guidePanel;

    private void Start()
    {
        guidePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !string.IsNullOrEmpty(targetSceneName))
        {
            IsPlayerInPortal = true;
            guidePanel?.SetActive(true);
            //SceneTransitionController.Instance.StartSceneTransition(targetSceneName);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !string.IsNullOrEmpty(targetSceneName))
        {
            IsPlayerInPortal = false;
            guidePanel?.SetActive(false);
            //SceneTransitionController.Instance.StartSceneTransition(targetSceneName);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneTransitionController.Instance.StartSceneTransition(targetSceneName);
        }
    }
}
