using System.Collections;
using UnityEngine;

public class EnemyBlueMage : Enemy
{
    [SerializeField] private Transform shootPoint;
    [SerializeField] private SpellManager spellManager;

    protected override IEnumerator HandleAttack()
    {
        alreadyAttacked = true;

        // Activar la animación de disparo
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }

        yield return new WaitForSeconds(0.5f); // Esperar antes de lanzar el proyectil

        // Lanzar el proyectil
        if (spellManager != null && shootPoint != null)
        {
            Vector3 direction = (player.position - shootPoint.position).normalized;
            spellManager.LaunchProjectile(shootPoint.position, direction);
        }

        if (animator != null)
        {
            animator.SetTrigger("Idle"); // Activar el trigger
        }


        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
    }
}