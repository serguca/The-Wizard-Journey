using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySkeleton : Enemy
{
    protected override IEnumerator HandleAttack()
    {
        alreadyAttacked = true;
        if (animator != null) animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);


        if (animator != null) animator.SetTrigger("Idle");
        agent.isStopped = true;
        animator.SetTrigger("Dizzy");
        yield return new WaitForSeconds(timeBetweenAttacks);
        agent.isStopped = false;
        alreadyAttacked = false;
    }

}
