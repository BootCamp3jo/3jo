﻿using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class APlayer : MonoBehaviour
{
    [SerializeField]
    protected PlayerData playerData;
    protected PlayerStateInScene playerStateInScene;
    private GameContext gameContext;


    protected virtual void Awake()
    {
        gameContext = GameManager.instance.gameContext;
        string curSceneName = SceneManager.GetActiveScene().name;
        // 해당 Scene 내용이 저장되어 있으면 saveData에서 상태 받아오기
        if (gameContext.IsSceneSaved(curSceneName))
        {
            this.playerData = gameContext.saveData.playerData;
            this.playerStateInScene = gameContext.saveData.sceneBundles[curSceneName].playerStateInScene;

            // 받아온 상태 처리
            gameObject.transform.position = new Vector3(playerStateInScene.posX, playerStateInScene.posY, playerStateInScene.posZ);
            /*
             * 
             * 
             * 
             * 
             */

            if (playerStateInScene.isPlayerExist == false)
            {
                Destroy(gameObject);
            }
        }
        // 아니면 새로 생성하기
        else
        {
            if (gameContext.playerData == null)
            {
                gameContext.playerData = playerData;
            }
            playerStateInScene = new();
        }
        playerStateInScene.isPlayerExist = true;
        gameContext.playerStateInScene = playerStateInScene;
    }

    protected virtual void Update()
    {
        // 지속적 동기화
        playerStateInScene.posX = gameObject.transform.position.x;
        playerStateInScene.posY = gameObject.transform.position.y;
        playerStateInScene.posZ = gameObject.transform.position.z;

        /*
         * 
         * 세이브 원하는 데이터 추가 시 동기화
         * 
         * 
         */
    }
}