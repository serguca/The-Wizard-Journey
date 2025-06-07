using System.Collections;
using UnityEngine;

public class SpiderEnemy : Enemy
{
    private GameObject UI;
    private bool secondPhase = false;
    protected override void Start()
    {
        base.Start();
        isStunneable = false;
        doesDissapear = false;
        legacyAnimations = true;
        UI = transform.Find("UI").gameObject;
        UI.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        if (!UI.activeSelf && hasLineOfSight)
        {
            UI.SetActive(true);
        }
    }

    protected override IEnumerator HandleAttack()
    {
        col = GetComponent<Collider>();
        attackCooldownActive = true;
        if (animator != null) animator.SetTrigger("Attack");

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = true;

        yield return new WaitForSeconds(1.5f);

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;

        if (animator != null && !isDead) animator.SetTrigger("Idle");
        attackCooldownActive = false;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (!secondPhase && health < maxHealth / 2f)
        {
            secondPhase = true;
        }
    }
    

}
