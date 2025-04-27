using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    [SerializeField] private LayerMask whatsIsGround, whatsIsPlayer;

    [Header("Patrolling")]
    [SerializeField] private bool doesItPatrols = false;
    [SerializeField] private float walkPointRange;
    private Vector3 walkPoint;
    private bool walkPointSet;

    [Header("Attacking")]
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;

    [Header("States")]
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInAttackRange;

    [Header("Shooting")]
    [SerializeField] private Transform shootPoint; // Punto de disparo del enemigo
    private SpellManager spellManager; // Referencia al SpellManager
    private Animator animator; // Referencia al Animator

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Obtener el Animator del enemigo

        // Buscar el SpellManager en la escena
        spellManager = FindFirstObjectByType<SpellManager>();
        if (spellManager == null)
        {
            Debug.LogError("No se encontró un SpellManager en la escena.");
        }
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatsIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatsIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
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
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position); // Detener el movimiento del enemigo
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition); // Mirar al jugador sin el eje y

        if (!alreadyAttacked)
        {
            StartCoroutine(HandleShooting());
        }
    }
    private IEnumerator HandleShooting()
    {
        alreadyAttacked = true;

        // Activar la animación de disparo
        if (animator != null)
        {
            animator.SetBool("IsShooting", true);
        }

        // Esperar un pequeño delay antes de lanzar el proyectil (sincronizado con la animación)
        yield return new WaitForSeconds(0.5f); // Ajusta este valor según la duración de la animación de disparo

        // Lanzar el proyectil
        ShootProjectile();

        // Volver a la animación de Idle
        if (animator != null)
        {
            animator.SetBool("IsShooting", false);
        }

        // Esperar el tiempo entre ataques
        yield return new WaitForSeconds(timeBetweenAttacks - 0.5f); // Resto del tiempo de espera

        alreadyAttacked = false;
    }

    private void ShootProjectile()
    {
        if (spellManager != null && shootPoint != null)
        {
            // Calcular la dirección hacia el jugador
            Vector3 direction = (player.position - shootPoint.position).normalized;

            // Usar el SpellManager para lanzar el proyectil
            spellManager.LaunchProjectile(shootPoint.position, direction);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}