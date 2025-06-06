using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpider : Enemy
{
    [SerializeField] private Weapon weapon;// Rango de ataque
    private float attackDuration = 3; // Duración del ataque
    protected override void Start()
    {
        base.Start();
        weapon.SetDamage(damage);
    }

    protected override IEnumerator HandleAttack()
    {
        attackCooldownActive = true;
        if (animator != null) animator.SetTrigger("Attack");

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = true;


        yield return new WaitForSeconds(0.5f);
        weapon.SetColliderActive(true);
        yield return new WaitForSeconds(0.5f);
        weapon.SetColliderActive(false);
        // yield return EnableColliderAndDisableAfterTime(1.25f);
        // yield return new WaitForSeconds(2f);

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;

        if (animator != null && !isDead) animator.SetTrigger("Idle");
        attackCooldownActive = false;
    }

    protected IEnumerator EnableColliderAndDisableAfterTime(float time)
    {
        yield return new WaitForSeconds(1f);
        weapon.SetColliderActive(true);
        yield return new WaitForSeconds(time);
        weapon.SetColliderActive(false);
    }
}
