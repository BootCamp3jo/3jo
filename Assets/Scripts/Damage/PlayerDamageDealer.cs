using npcDialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageDealer : A_BaseDamageDealer
{
    private HashSet<DialogueNpc> currentNpc = new HashSet<DialogueNpc>();

    protected override void Update()
    {
        if (isAttacking)
        {
            CollectAttackTargets();
        }
    }

    public override void StartAttack()
    {
        isAttacking = true;
        currentTargets.Clear();
    }

    public override void EndAttack()
    {
        isAttacking = false;
    }

    public void CollectAttackTargets()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, hitRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<MonsterBase>(out MonsterBase enemy))
            {
                currentTargets.Add(enemy);
            }
            if (hit.TryGetComponent<DialogueNpc>(out DialogueNpc npc))
            {
                currentNpc.Add(npc);
            }
        }
    }

    public override void Attack()
    {
        foreach (var target in currentTargets)
        {
            target.GetDamage(attackDamage);
            Debug.Log($"Hit {target.name} for {attackDamage} damage.");
        }
        foreach (var npc in currentNpc)
        {
            npc.InteractiveNPC();

        }

        currentTargets.Clear();
    }
}
