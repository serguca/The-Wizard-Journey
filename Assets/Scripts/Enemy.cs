using System.Collections;
using System.Runtime.CompilerServices;
using MagicPigGames;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class Enemy : Character
{
    protected NavMeshAgent agent;
    protected Transform player;
    private Collider col;

    private LayerMask whatsIsGround, whatsIsPlayer;
    protected Animator animator;

    [Header("Patrolling")]
    protected bool doesItPatrols = false;
    protected Vector3 walkPoint;
    protected bool walkPointSet;

    [Header("Attacking")]
    [SerializeField] protected float timeBetweenAttacks = 2f;
    protected bool attackCooldownActive = false;
    [Header("States")]
    [SerializeField] protected float sightRange, attackRange;
    protected bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();
        whatsIsGround = LayerMask.GetMask("Ground");
        whatsIsPlayer = LayerMask.GetMask("Player");
    }

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        EventManager.DamageEnemy += TakeDamage;
    }

    private void OnDisable()
    {
        EventManager.DamageEnemy -= TakeDamage;
    }

    private void Update()
    {
        if (isDead) return; // Bloquea toda la lógica si está muerto excepto los eventos
        if (health <= 0f) Die();
        if (attackCooldownActive || hitCooldownActive) return; //TODO: hacer esto mejor

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatsIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatsIsPlayer);

        if (playerInSightRange && playerInAttackRange) AttackPlayer();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        // if (!playerInSightRange && !playerInAttackRange) Patrolling();
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

    private void TakeDamage(float damage)
    {
        if (hitCooldownActive || isDead) return;

        health -= damage;
        SetProgressBar(health);

        if (health <= 0f && !isDead)
        {
            Die();
            return;
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

    protected void ResetAllTriggers()
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
            Debug.Log("MUERTEADO");
            //ResetAllTriggers(); //ya no hace falta con crossfade
            animator.CrossFade("Death", 0.25f, 0, 0f); //evitamos problemas con exit time
        }
        if (col != null) col.enabled = false;
        if (agent != null) agent.enabled = false;

        StartCoroutine(DisappearAfterSeconds(30f));
        StartCoroutine(HealthBarDissapear(2f));
    }

    private IEnumerator HealthBarDissapear(float time)
    {
        yield return new WaitForSeconds(time);
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
        float walkPointRange = 10f; // Rango de búsqueda de puntos de patrulla
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatsIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        agent.isStopped = false;
        if (animator != null)
        {
            ResetAllTriggers();
            animator.SetBool("IsWalking", true);
        }

        SmoothLookAtPlayer();
    }

    private void LookPlayer()
    {
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        // transform.LookAt(targetPosition);
        Quaternion rotacionDeseada = Quaternion.LookRotation(targetPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, 1 * Time.deltaTime);
    }

    protected bool IsFacingPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; // Ignorar la diferencia en altura
        Vector3 forward = transform.forward;

        float angle = Vector3.Angle(forward, directionToPlayer);
        return angle <= 15f;
    }


    private void AttackPlayer()
    {
        // Primero gira hacia el jugador
        SmoothLookAtPlayer();

        // Detiene el movimiento
        agent.SetDestination(transform.position);
        if (animator != null) animator.SetBool("IsWalking", false);

        // Solo ataca si está mirando al jugador y no está en cooldown
        if (!attackCooldownActive && IsFacingPlayer())
        {
            StartCoroutine(HandleAttack());
        }
    }

    private void SmoothLookAtPlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0; // Ignorar la diferencia en altura

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3f * Time.deltaTime);
        }
    }

    protected abstract IEnumerator HandleAttack(); // Método abstracto para que cada enemigo implemente su ataque

}