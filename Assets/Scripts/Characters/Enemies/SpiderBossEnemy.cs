using System.Collections;
using UnityEngine;

public class SpiderBossEnemy : Enemy
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject eggs;
    private EggController[] eggControllers; // Referencia a los scripts EggController en los hijos
    private GameObject UI;
    private bool secondPhase = false;
    protected override void Start()
    {
        base.Start();
        isStunneable = false;
        doesDissapear = false;
        useDeathTrigger = true;
        UI = transform.Find("UI").gameObject;
        UI.SetActive(false);
        weapon.SetDamage(damage);
        eggControllers = eggs.GetComponentsInChildren<EggController>(true);
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

        yield return new WaitForSeconds(0.5f);
        weapon.SetColliderActive(true);
        yield return new WaitForSeconds(0.5f);
        weapon.SetColliderActive(false);
        yield return new WaitForSeconds(0.5f);

        if (agent != null && agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;

        if (animator != null && !isDead) animator.SetTrigger("Idle");
        attackCooldownActive = false;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (!secondPhase && health < maxHealth * 0.70f)
        {
            secondPhase = true;
            StartCoroutine(SecondPhase());
        }
    }

    private IEnumerator SecondPhase()
    {
        foreach (var eggController in eggControllers)
        {
            eggController.Destroy();
            yield return new WaitForSeconds(1f); // Delay between egg destruction
        }
    }
    
}
