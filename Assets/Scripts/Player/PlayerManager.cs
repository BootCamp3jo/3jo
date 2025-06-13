using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [Header("Player Prefab")]
    [SerializeField] public GameObject playerPrefab;

    [Header("Player Components")]
    [SerializeField] public PlayerMovement playerMovement;
    [SerializeField] public PlayerAnimationHandler playerAnimationHandler;
    [SerializeField] public PlayerStatData playerStatData;
    [SerializeField] public PlayerCombatHandler playerCombatHandler;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimationHandler = GetComponentInChildren<PlayerAnimationHandler>();
        playerStatData = GetComponent<PlayerStatData>();
        playerCombatHandler = GetComponent<PlayerCombatHandler>();
    }

}
