using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Transform player;
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

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatsIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatsIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    protected virtual void Patrolling()
    {
        if (doesItPatrols)
        {
            if (!walkPointSet) SearchWalkPoint();
            if (walkPointSet) agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
        }
    }

    protected virtual void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatsIsGround))
            walkPointSet = true;
    }

    protected virtual void ChasePlayer()
    {
        agent.SetDestination(player.position);
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    protected virtual void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
        }

        if (!alreadyAttacked)
        {
            StartCoroutine(HandleAttack());
        }
    }

    protected abstract IEnumerator HandleAttack(); // Método abstracto para que cada enemigo implemente su ataque
}