using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageDealer : A_BaseDamageDealer
{
    protected override void Update()
    {
        base.Update();
        if (isAttacking)
        {
            CollectAttackTargets();
        }
    }

    public override void Attack()
    {
        base.Attack();
        foreach (var target in currentTargets)
        {
            target.TakeDamage(attackDamage);
            Debug.Log($"Hit {target.name} for {attackDamage} damage.");
        }

        currentTargets.Clear();
    }

    public void CollectAttackTargets()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, hitRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
            {
                currentTargets.Add(enemy);
            }
        }
    }
}
