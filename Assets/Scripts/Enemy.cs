using System.Collections;
using MagicPigGames;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Character
{
    protected Collider col;
    protected NavMeshAgent agent;
    protected Transform player;
    [SerializeField] private HorizontalProgressBar healthBar;
    private float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] private bool hitCooldownActive = false;
    protected bool isDead = false;

    [SerializeField] protected LayerMask whatsIsGround, whatsIsPlayer;

    [Header("Patrolling")]
    [SerializeField] protected bool doesItPatrols = false;
    [SerializeField] protected float walkPointRange;
    protected Vector3 walkPoint;
    protected bool walkPointSet;

    [Header("Attacking")]
    [SerializeField] protected float timeBetweenAttacks = 2f;
    protected bool alreadyAttacked = false;
    [Header("States")]
    [SerializeField] protected float sightRange, attackRange;
    protected bool playerInSightRange, playerInAttackRange;
    protected Animator animator;
    private void Awake()
    {
        EventManager.DamageEnemy += TakeDamage;
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();

    }

    private void Update()
    {
        if (isDead) return; // Bloquea toda la lógica si está muerto excepto los eventos
        //if (health <= 0f) Die();
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatsIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatsIsPlayer);

        if (alreadyAttacked) return; //TODO: hacer esto mejor

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) StartCoroutine(ChasePlayer());
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

    }
    private IEnumerator ColliderCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        hitCooldownActive = false;
    }

    private void TakeDamage(float damage)
    {
        if (hitCooldownActive || isDead) return;
        hitCooldownActive = true;

        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Hit");


        StartCoroutine(ColliderCooldown());
        StartCoroutine(ResumeChaseAfterDelay(2f));
        
        health -= damage;
        setProgressBar(health);
        if (health <= 0f && !isDead) Die();
    }

    private IEnumerator ResumeChaseAfterDelay(float delay)
    {
        if (isDead) yield break;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        yield return new WaitForSeconds(delay);
        //if (agent!=null) animator.SetTrigger("Idle");
        if (agent.isActiveAndEnabled) agent.isStopped = false;
    }

    private void setProgressBar(float health)
    {
        if (health > 0)
            healthBar.SetProgress(health / maxHealth);
        else
            healthBar.SetProgress(0);
    }

    private void ResetAllTriggers()
    {
        if (animator == null) return;
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }

    private void Die()
    {
        isDead = true;
        if (animator != null)
        {
            ResetAllTriggers();
            animator.SetTrigger("Death"); // Usa trigger, no bool
            StartCoroutine(DisableAnimator());
        }
        if (col != null) col.enabled = false;
        if (agent != null) agent.enabled = false;

        StartCoroutine(DisappearAfterSeconds(30f));
        StartCoroutine(healthBarDissapear());
    }

    private IEnumerator healthBarDissapear()
    {
        yield return new WaitForSeconds(1f);
        healthBar.gameObject.SetActive(false);
    }

    private IEnumerator DisappearAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }

    private IEnumerator DisableAnimator()
    {
        yield return new WaitForSeconds(10f);
        if (animator != null) animator.enabled = false;
    }

    private void Patrolling()
    {
        if (doesItPatrols)
        {
            if (!walkPointSet) SearchWalkPoint();
            if (walkPointSet) agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatsIsGround))
            walkPointSet = true;
    }

    private IEnumerator ChasePlayer()
    {
        yield return new WaitForSeconds(0.4f);
        if (animator != null) animator.SetTrigger("Walking");
        StartCoroutine(LookPlayer());
        yield return new WaitForSeconds(0.1f);
        agent.SetDestination(player.position);

    }

    private IEnumerator LookPlayer()
    {
        float duration = 0.1f;
        float elapsed = 0f;
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }


    private void AttackPlayer()
    {
        StartCoroutine(LookPlayer());
        agent.SetDestination(transform.position);
        if (animator != null) animator.SetBool("IsWalking", false);

        if (!alreadyAttacked) StartCoroutine(HandleAttack());
    }

    protected abstract IEnumerator HandleAttack(); // Método abstracto para que cada enemigo implemente su ataque

}