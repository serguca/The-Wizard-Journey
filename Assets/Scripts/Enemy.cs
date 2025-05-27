using System.Collections;
using System.Runtime.CompilerServices;
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
        EventManager.DamageEnemy += (damage) => StartCoroutine(TakeDamage(damage));
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
    }

    private void Update()
    {
        if (isDead) return; // Bloquea toda la lógica si está muerto excepto los eventos
        if (health <= 0f) Die();
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatsIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatsIsPlayer);

        if (alreadyAttacked || hitCooldownActive) return; //TODO: hacer esto mejor

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();

    }

    private IEnumerator ColliderCooldown()
    {
        float cooldownTime = 1f;
        yield return new WaitForSeconds(cooldownTime);
        hitCooldownActive = false;
        if (!isDead)
        {
            animator.SetTrigger("Idle"); //evitamos problemas isWalking, importante cooldown 1s
            Debug.Log("Cooldown finished, idle");  
        } 
    }

    private IEnumerator TakeDamage(float damage)
    {
        if (hitCooldownActive || isDead) yield break;

        health -= damage;
        SetProgressBar(health);

        if (health <= 0f && !isDead)
        {
            Die();
            yield break;
        }

        hitCooldownActive = true;
        if (animator != null) animator.SetTrigger("Hit");
        StartCoroutine(ColliderCooldown());
        agent.isStopped = true;
    }

    private void SetProgressBar(float health)
    {
        if (health > 0)
            healthBar.SetProgress(health / maxHealth);
        else healthBar.SetProgress(0);
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
            Debug.Log("MUERTEADO");
            animator.SetTrigger("Death");
        }
        if (col != null) col.enabled = false;
        if (agent != null) agent.enabled = false;

        StartCoroutine(DisappearAfterSeconds(30f));
        StartCoroutine(HealthBarDissapear());
    }

    private IEnumerator HealthBarDissapear()
    {
        yield return new WaitForSeconds(1f);
        healthBar.gameObject.SetActive(false);
    }

    private IEnumerator DisappearAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
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

    private void ChasePlayer()
    {
        Debug.Log("Chasing player");
        agent.SetDestination(player.position);
        agent.isStopped = false;
        if (animator != null)
        {
            ResetAllTriggers();
            animator.SetBool("IsWalking", true);
        } 

        LookPlayer();
    }

    private void LookPlayer()
    {
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition);
    }


    private void AttackPlayer()
    {
        LookPlayer();
        agent.SetDestination(transform.position);
        if (animator != null) animator.SetBool("IsWalking", false);

        if (!alreadyAttacked) StartCoroutine(HandleAttack());
    }

    protected abstract IEnumerator HandleAttack(); // Método abstracto para que cada enemigo implemente su ataque

}