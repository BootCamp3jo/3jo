using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkillPattern02 : MonoBehaviour
{
    public int damage = 30;
    public float dealDamageInterval = 0.1f;
    public LayerMask targetLayer;
    public Vector2 damageAreaSize = new Vector2(5f, 5.5f);

    private Coroutine damageCoroutine;
    private WaitForSeconds waitForSeconds;

    private void OnEnable02()
    {
        waitForSeconds = new WaitForSeconds(dealDamageInterval);
    }

    public void StartDamage02()
    {
        if (damageCoroutine == null)
            damageCoroutine = StartCoroutine(DamageLoop02());
    }

    public void StopDamage02()
    {
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
    }

    private IEnumerator DamageLoop02()
    {
        while (true)
        {
            // 적 정보를 모두 가져온다
            Collider2D[] targets = Physics2D.OverlapBoxAll(transform.position, damageAreaSize, 0f, targetLayer);

            // 데미지를 준다
            foreach (var target in targets)
            {
                EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
                if (enemyHealth != null) 
                    enemyHealth.TakeDamage(damage);
            }

            yield return waitForSeconds;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, damageAreaSize);
    }
}
