using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class Player : APlayer
{
    [SerializeField] private PlayerMovement playerMovement;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        playerMovement = PlayerManager.Instance.playerMovement;
    }

    public void OnHitByEnemy()
    {
        if (playerMovement.JustDodgeWindow)
        {
            Debug.Log("저스트닷지 발동!");
        }
        else
        {
            Debug.Log("피격!");
        }
    }
}
