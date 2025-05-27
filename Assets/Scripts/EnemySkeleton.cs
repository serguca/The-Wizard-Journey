using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySkeleton : Enemy
{
    [SerializeField] private float attackDuration = 0.5f; // Duración del ataque
    protected override IEnumerator HandleAttack()
    {
        alreadyAttacked = true;
        if (animator != null) animator.SetTrigger("Attack");

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = true;

        yield return new WaitForSeconds(attackDuration);

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;

        if (animator != null && !isDead) animator.SetTrigger("Idle");
        alreadyAttacked = false;
    }

}
