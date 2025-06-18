using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Portal : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    public bool IsPlayerInPortal = false;
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
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerInPortal)
        {
            foreach (Action action in actionBeforeSceneTransitionList)
            {
                action.Invoke();
            }

            SceneTransitionController.Instance.StartSceneTransition(targetSceneName);
        }
    }

    public void MoveScene()
    {
        foreach (Action action in actionBeforeSceneTransitionList)
        {
            action.Invoke();
        }
        SceneTransitionController.Instance.StartSceneTransition(targetSceneName);
    }
}
