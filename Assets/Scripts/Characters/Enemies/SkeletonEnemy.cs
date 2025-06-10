using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonEnemy : Enemy
{
    [SerializeField] private Weapon weapon;
    [SerializeField] protected AudioClip attackSound2;
    private float attackDuration = 3; // Duración del ataque
    protected override void Start()
    {
        base.Start();
        useDeathTrigger = false;
        weapon.SetDamage(damage);
    }

    protected override IEnumerator HandleAttack()
    {
        attackCooldownActive = true;
        if (animator != null) animator.SetTrigger("Attack");

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = true;

        yield return new WaitForSeconds(0.1f);
        SoundManager.Instance.PlaySound(attackSound, transform.position);
        yield return new WaitForSeconds(0.4f);
        SoundManager.Instance.PlaySound(attackSound2, transform.position);
        weapon.SetColliderActive(true);
        yield return new WaitForSeconds(0.5f);
        weapon.SetColliderActive(false);

        ResetAllTriggers();

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;

        if (animator != null && !isDead) animator.SetTrigger("Idle");
            attackCooldownActive = false;
    }

}
