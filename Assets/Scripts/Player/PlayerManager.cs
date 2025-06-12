using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public PlayerMovement playerMovement;
    [SerializeField] public PlayerAnimationHandler playerAnimationHandler;  

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        playerMovement = playerPrefab.GetComponent<PlayerMovement>();
        playerAnimationHandler = playerPrefab.GetComponent<PlayerAnimationHandler>();
    }

}
