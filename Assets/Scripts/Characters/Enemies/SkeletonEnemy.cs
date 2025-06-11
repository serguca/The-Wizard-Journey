using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonEnemy : Enemy
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private AudioClip swordSwingSound;
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

        yield return new WaitForSeconds(0.1f);
        if (isDead) yield break;

        SoundManager.Instance.PlaySound(attackSound, transform.position);
        yield return new WaitForSeconds(0.4f);
        if (isDead) yield break;

        SoundManager.Instance.PlaySound(swordSwingSound, transform.position, 1f);
        weapon.SetColliderActive(true);

        yield return new WaitForSeconds(0.5f);
        if (isDead) yield break;

        weapon.SetColliderActive(false);

        // ResetAllTriggers();

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;

        if (animator != null && !isDead) animator.SetTrigger("Idle");
            attackCooldownActive = false;
    }

}
