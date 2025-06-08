using System.Collections;
using UnityEngine;

public class SpiderEnemy : Enemy
{
    [SerializeField] private Weapon weapon;

    protected override void Start()
    {
        base.Start();
        isStunneable = false;
        useDeathTrigger = true;
        weapon.SetDamage(damage);
    }
    protected override IEnumerator HandleAttack()
    {
        col = GetComponent<Collider>();
        attackCooldownActive = true;
        if (animator != null) animator.SetTrigger("Attack");

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = true;

        weapon.SetColliderActive(true);
        yield return new WaitForSeconds(0.5f);
        weapon.SetColliderActive(false);
        yield return new WaitForSeconds(0.8f);

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;

        if (animator != null && !isDead) animator.SetTrigger("Idle");
        attackCooldownActive = false;
    }


    
}
