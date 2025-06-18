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
        AudioManager.instance.PlaySFX(SFXType.Attack, 0.7f, 1.0f);
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

            // 공격 시 궁극기 게이지 증가
            if (UIManager.Instance.ultGuageBarManager.IsUltGuageActive())
                UIManager.Instance.ultGuageBarManager.IncreaseUltGuage(0.05f); 

            Debug.Log($"Hit {target.name} for {attackDamage} damage.");
        }
        foreach (var npc in currentNpc)
        {
            npc.InteractiveNPC();

        }

        currentTargets.Clear();
    }
}
