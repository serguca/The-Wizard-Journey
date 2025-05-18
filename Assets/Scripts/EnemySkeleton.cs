using System.Collections;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    protected override IEnumerator HandleAttack()
    {
        alreadyAttacked = true;
        if (animator != null) animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);


        if (animator != null) animator.SetTrigger("Idle");
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
    }
}
