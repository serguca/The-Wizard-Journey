using System.Collections;
using MagicPigGames;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected Collider col;
    protected NavMeshAgent agent;
    protected Transform player;
    [SerializeField] private HorizontalProgressBar healthBar;
    private float health;
    [SerializeField] protected float maxHealth;
    [SerializeField] private bool hitCooldownActive = false;
    private bool isDead = false;

    [SerializeField] protected LayerMask whatsIsGround, whatsIsPlayer;

    [Header("Patrolling")]
    [SerializeField] protected bool doesItPatrols = false;
    [SerializeField] protected float walkPointRange;
    protected Vector3 walkPoint;
    protected bool walkPointSet;

    [Header("Attacking")]
    [SerializeField] protected float timeBetweenAttacks = 2f;
    protected bool alreadyAttacked;

    [Header("States")]
    [SerializeField] protected float sightRange, attackRange;
    protected bool playerInSightRange, playerInAttackRange;

    protected Animator animator;

    private void Awake()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();

    }

    private void Update()
    {
        if (health <= 0f && !isDead) Die();
        if (isDead) return; // Bloquea toda la lógica si está muerto
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatsIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatsIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(hitCooldownActive) return;
        if (other.CompareTag("Hit"))
        {
            if (animator != null) animator.SetTrigger("Hit");
            TakeDamage(10f);
            Debug.Log("El enemigo ha sido golpeado por un proyectil.");
        }
    }

    private IEnumerator ColliderCooldown()
    {
        yield return new WaitForSeconds(0.1f);
        hitCooldownActive = false;
    }

    private void TakeDamage(float damage)
    {
        hitCooldownActive = true;
        StartCoroutine(ColliderCooldown());
        health -= damage;
        healthBar.SetProgress(health / maxHealth); // Actualiza la barra de salud
        if (health <= 0f && !isDead) Die();
    }

    private void Die()
    {
        isDead = true;

        if (animator != null) animator.SetTrigger("Death");
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
        agent.SetDestination(player.position);
        if (animator != null) animator.SetBool("IsWalking", true);

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