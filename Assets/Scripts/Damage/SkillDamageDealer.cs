using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamageDealer : A_BaseDamageDealer
{
    protected override void Update()
    {
        base.Update();
        if (isAttacking)
        {
            Attack();
        };
    }

    public override void Attack()
    {
        base.Attack();
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, hitRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out EnemyHealth enemy) && !currentTargets.Contains(enemy))
            {
                enemy.TakeDamage(attackDamage);
                currentTargets.Add(enemy);
                Debug.Log($"Hit {enemy.name} for {attackDamage} damage.");
            }
        }
    }
}
