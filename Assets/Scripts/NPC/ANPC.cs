using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ANPC : MonoBehaviour
{
    [SerializeField]
    public NPCData npcData;
    private GameContext gameContext;
    public bool isCreatedBySceneLoader = false;

    protected virtual void Awake()
    {
        gameContext = GameManager.instance.gameContext;
    }

    protected virtual void Start()
    {
        string curSceneName = SceneManager.GetActiveScene().name;
        // 해당 Scene 내용이 저장되어 있으면 saveData에서 상태 받아오기(전달 주체는 SceneLoader 지만 신경 쓸 필요 없음)
        if (gameContext.IsSceneSaved(curSceneName))
        {
            if (isCreatedBySceneLoader == false)
            {
                Destroy(gameObject);
                return;
            }
            // 받아온 상태 처리
            gameObject.transform.position = new Vector3(npcData.posX, npcData.posY, npcData.posZ);

            /*
             * 
             *
             * 
             * 
             */
        }
        // 동기화 위해서 gameContext에 등록하기
        gameContext.RegisterNPC(gameObject, npcData);
    }

    protected virtual void Update()
    {
        // 지속적 동기화
        npcData.posX = gameObject.transform.position.x;
        npcData.posY = gameObject.transform.position.y;
        npcData.posZ = gameObject.transform.position.z;

        /*
         * 
         * 세이브 원하는 데이터 추가 시 동기화
         * 
         * 
         */
    }
}
