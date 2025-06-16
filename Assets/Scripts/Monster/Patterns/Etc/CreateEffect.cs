using UnityEngine;

public class CreateEffect : MonoBehaviour
{
    [SerializeField] Transform effectPrefab;
    private void Awake()
    {
        if (effectPrefab == null)
            Debug.Log("이펙트 프리팹이 할당되지 않았습니다.");
        else
            Instantiate(effectPrefab, transform);
    }
}
