using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BlueMageEnemy : Enemy
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private SpellManager spellManager;
    protected override IEnumerator HandleAttack()
    {
        Debug.Log("EnemyBlueMage: HandleAttack called");
        attackCooldownActive = true;

        if (animator != null) animator.SetTrigger("Shoot");
        Debug.Log("EnemyBlueMage: Shoot animation triggered");
        yield return new WaitForSeconds(0.5f); // Tiempo antes de disparar
                ResetAllTriggers();

        if (spellManager != null && shootPoint != null)
        {
            spellManager.SetDamage(damage);
            Vector3 direction = (player.position - shootPoint.position).normalized;
            spellManager.LaunchProjectile(shootPoint.position, direction, damage, this.tag);
        }

        yield return new WaitForSeconds(1f); // Tiempo después de disparar

        
        if (animator != null && !isDead) animator.SetTrigger("Idle");
        attackCooldownActive = false;
    }
}