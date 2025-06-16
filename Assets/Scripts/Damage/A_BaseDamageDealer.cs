using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_BaseDamageDealer : MonoBehaviour
{
    public Transform attackPoint;
    public float hitRadius;
    public LayerMask enemyLayer;
    public int attackDamage;

    public HashSet<MonsterBase> currentTargets = new HashSet<MonsterBase>();
    public bool isAttacking = false;

    protected virtual void Update()
    {
        
    }

    public virtual void StartAttack()
    {
        isAttacking = true;
        currentTargets.Clear();
    }

    public virtual void Attack()
    {
        
    }

    public virtual void EndAttack()
    {
        isAttacking = false;
    }

    protected void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, hitRadius);
        }
    }
}
