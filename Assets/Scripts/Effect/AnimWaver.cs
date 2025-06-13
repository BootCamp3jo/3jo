using UnityEngine;

public class AnimWaver : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if(animator!= null)
            animator.speed = Random.Range(0.8f, 1.2f); // 흔들리는 속도를 풀마다 다르게

        // Z-offset 처리 (깊이 문제 해결)
        float zOffset = transform.position.y * 0.01f;
        transform.position = new Vector3(transform.position.x, transform.position.y, zOffset);

        // SpriteRenderer order 조정
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = -(int)(transform.position.y * 100);
    }
}
