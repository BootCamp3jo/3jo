using System;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    public bool isPlayerInPortal = false;
    [SerializeField] public GameObject guidePanel;
    [SerializeField] public List<Action> actionBeforeSceneTransitionList = new List<Action>();

    private void Start()
    {
        guidePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !string.IsNullOrEmpty(targetSceneName))
        {
            isPlayerInPortal = true;
            guidePanel?.SetActive(true);
            //SceneTransitionController.Instance.StartSceneTransition(targetSceneName);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !string.IsNullOrEmpty(targetSceneName))
        {
            isPlayerInPortal = false;
            guidePanel?.SetActive(false);
            //SceneTransitionController.Instance.StartSceneTransition(targetSceneName);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isPlayerInPortal)
        {
            for (int i = 0; i < actionBeforeSceneTransitionList.Count; i++)
            {
                actionBeforeSceneTransitionList[i]?.Invoke();
            }
            SceneTransitionController.Instance.StartSceneTransition(targetSceneName);
        }
    }
}
