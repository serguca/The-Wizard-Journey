using System.Collections;
using UnityEngine;

public class SpiderEnemy : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override IEnumerator HandleAttack()
    {
        setStunneable(false);
        col = GetComponent<Collider>();
        attackCooldownActive = true;
        if (animator != null) animator.SetTrigger("Attack");

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = true;


        // yield return new WaitForSeconds(0.5f);
        // weapon.SetColliderActive(true);
        // yield return new WaitForSeconds(0.5f);
        // weapon.SetColliderActive(false);
        // yield return EnableColliderAndDisableAfterTime(1.25f);
        yield return new WaitForSeconds(1.5f);

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;

        if (animator != null && !isDead) animator.SetTrigger("Idle");
        attackCooldownActive = false;
    }
}
