using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [Header("Player Prefab")]
    [SerializeField] public GameObject playerPrefab;

    [Header("Player Components")]
    [SerializeField] public Player player;
    [SerializeField] public PlayerData playerData;
    [SerializeField] public PlayerMovement playerMovement;
    [SerializeField] public PlayerAnimationHandler playerAnimationHandler;
    [SerializeField] public PlayerCombatHandler playerCombatHandler;
    [SerializeField] public PlayerStatHandler playerStatHandler;
    [SerializeField] public PlayerEffectController playerEffectController;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        player = GetComponent<Player>();
        playerData = player.PlayerData;
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimationHandler = GetComponentInChildren<PlayerAnimationHandler>();
        playerCombatHandler = GetComponent<PlayerCombatHandler>();
        playerStatHandler = GetComponent<PlayerStatHandler>();
        playerEffectController = GetComponent<PlayerEffectController>();
    }

}
