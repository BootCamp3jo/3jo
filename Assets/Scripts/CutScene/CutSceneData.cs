using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New CutSceneData", menuName = "CutSceneData")]
public class CutSceneData : ScriptableObject
{
    [Header("컷씬 리스트")]
    public List<CutData> cutList;
}

[System.Serializable]
public class CutData
{
    [Header("false : 카메라컷 , true : 대화 컷")]
    public bool isDialogue; // 카메라 이벤트인지 다이얼로그 이벤트인지 확인
    [Header("카메라 컷")]
    public CameraSceneData cameraSceneData;
    [Header("대화 컷")]
    public DialogueSceneData dialogueSceneData;
}

[System.Serializable]
public class CameraSceneData
{
    [Header("카메라 움직임 포인트")]
    public List<Vector3> cameraWayPointList;
    public float cutTime;
    public bool isEvent = false; // 컷씬 이벤트 있을지
    public int eventCode = -1; // 이벤트 없을 시 -1
    public float delayTime; // 컷씬 후 다음으로 넘어가기 전 딜레이 타임
}

[System.Serializable]
public class DialogueSceneData
{
    public List<DialogueData> dialogueList;
}

[System.Serializable]
public enum DialogueType
{
    Left,
    Center,
    Right
}

[System.Serializable]
public class DialogueData
{
    [Header("왼쪽(상대방), 중앙(해설), 오른쪽(플레이어)")]
    public DialogueType dialogueType; // 왼쪽(상대방), 중앙(해설), 오른쪽(플레이어)
    public Sprite portrait; // 초상화
    public string title; // 이름
    public string content; // 말풍선 내용
    [Header("말풍선 코드")]
    public int dialogueCode; // 말풍선 코드

    [Header("false : 일반 대화, true : 선택지 대화")]
    public bool isSelectDialogue; // 선택 다이얼로그 인지
    [Header("선택지 대화 리스트")]
    public List<SubDialogueData> subDialogueDataList; // 선택 다이얼로그
}

[System.Serializable]
public class SubDialogueData
{
    public string content;
    [Header("다음 나올 말풍선")]
    public int nextDialogue; // 선택 시 다음 말풍선 코드
}
