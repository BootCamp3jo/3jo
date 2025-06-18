using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    Transform target;

    // 타겟이라고는 해도.. 지금은 플레이어 뿐
    void Start()
    {
        target = PlayerManager.Instance.player.transform;
        Vector3 direction = (target.position - transform.position).normalized;
        transform.right = direction; // Z축 기준 회전 (2D 캐릭터가 오른쪽을 앞방향으로 쓴 경우)
    }
}
