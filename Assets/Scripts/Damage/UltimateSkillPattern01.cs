using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateSkillPattern01 : MonoBehaviour
{
    public GameObject rootGO;
    public int damage = 20;
    public float dealDamageInterval = 0.5f;
    public LayerMask targetLayer;
    public Vector2 damageAreaSize = new Vector2(5f, 3.5f);
    public Vector2 damageAreaOffset = Vector2.zero;

    private Coroutine damageCoroutine;
    private WaitForSeconds waitForSeconds;

    private void OnEnable()
    {
        waitForSeconds = new WaitForSeconds(dealDamageInterval);
    }

    public void StartDamage01()
    {
        if (damageCoroutine == null)
            damageCoroutine = StartCoroutine(DamageLoop01());
    }

    public void StopDamage01()
    {
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
    }

    public void DestorySelf()
    {
        Destroy(rootGO);
    }

    private IEnumerator DamageLoop01()
    {
        while (true)
        {
            // 적 정보를 모두 가져온다
            Collider2D[] targets = Physics2D.OverlapBoxAll(transform.position, damageAreaSize, 0f, targetLayer);

            // 데미지를 준다
            foreach (var target in targets)
            {
                MonsterBase enemyHealth = target.GetComponent<MonsterBase>();
                if (enemyHealth != null) 
                    enemyHealth.GetDamage(damage);
            }

            yield return waitForSeconds;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 boxCenter = (Vector2) transform.position + damageAreaOffset;
        Gizmos.DrawWireCube(boxCenter, damageAreaSize);
    }
}
