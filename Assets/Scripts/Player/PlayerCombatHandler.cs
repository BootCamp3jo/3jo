using npcDialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatHandler : MonoBehaviour
{
    public Transform attackPoint;
    public float hitRadius;
    public LayerMask enemyLayer;
    public int attackDamage;

    private HashSet<MonsterBase> currentTargets = new HashSet<MonsterBase>();
    private HashSet<DialogueNpc> currentNpc = new HashSet<DialogueNpc>();
    private bool isAttacking = false;

    private void Update()
    {
        if (isAttacking)
        {
            CollectAttackTargets();
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
        currentTargets.Clear();
    }

    public void EndAttack()
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

    public void CheckHit()
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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, hitRadius);
        }
    }
}
