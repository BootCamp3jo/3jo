using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    [SerializeField] private GameObject ultGuageUI;

    private void OnEnable()
    {
        AudioManager.instance.PlayBGM(BGMType.Battle);
    }

    private void EnableUltGuageUI()
    {
        // 만약 저장되어있는 스킬 데이터 중 궁극기가 언락되어있다면

    }
}
