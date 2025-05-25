using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBlueMage : Enemy
{   
    [SerializeField] private Transform shootPoint;
    [SerializeField] private SpellManager spellManager;

    protected override IEnumerator HandleAttack()
    {
        alreadyAttacked = true;
        if (animator != null) animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(1f);

        if (spellManager != null && shootPoint != null)
        {
            spellManager.SetDamage(damage);
            Vector3 direction = (player.position - shootPoint.position).normalized;
            spellManager.LaunchProjectile(shootPoint.position, direction, damage);
        }

        if (animator != null && !isDead) animator.SetTrigger("Idle");
        alreadyAttacked = false;
    }
}