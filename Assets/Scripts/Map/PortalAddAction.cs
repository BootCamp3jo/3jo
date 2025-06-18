using UnityEngine;

public class PortalAddAction : MonoBehaviour
{
    // Start is called before the first frame update
    public Portal portal;
    void Start()
    {
        portal?.actionBeforeSceneTransitionList?.Add(ClearDataBeforeUseBossClearPortal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearDataBeforeUseBossClearPortal()
    {
        DataManager.Instance.gameContext.ClearCurSceneBundle();
        DataManager.Instance.gameContext.DontSaveCurSceneBundle();
    }
}
