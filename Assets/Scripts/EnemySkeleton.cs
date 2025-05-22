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
        yield return new WaitForSeconds(attackDuration);


        //if (animator != null) animator.SetTrigger("Idle");
        agent.isStopped = true;
        //yield return new WaitForSeconds(attackDuration);
        animator.SetTrigger("Dizzy");
        yield return new WaitForSeconds(timeBetweenAttacks);
        agent.isStopped = false;
        alreadyAttacked = false;
    }

}
