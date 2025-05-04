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
    [SerializeField] private SpellManager spellManager; // Referencia al SpellManager
    private Animator animator; // Referencia al Animator

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Obtener el Animator del enemigo

        // Buscar el SpellManager en la escena
        //spellManager = FindFirstObjectByType<SpellManager>();
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

    private void OnTriggerEnter(Collider other)
    {
    // Verificar si el objeto que colisionó es un proyectil
    if (other.CompareTag("Hit"))
    {
        // Activar la animación de "hit"
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        // Opcional: Desactivar el proyectil después de la colisión
        //other.gameObject.SetActive(false);

        Debug.Log("El enemigo ha sido golpeado por un proyectil.");
    }
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
        animator.SetBool("IsWalking", true);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position); // Detener el movimiento del enemigo
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition); // Mirar al jugador sin el eje y

        animator.SetBool("IsWalking", false);

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
            animator.SetTrigger("Shoot"); // Activar el trigger
        }

        yield return new WaitForSeconds(0.5f); // Esperar un tiempo antes de lanzar el proyectil

        // Lanzar el proyectil
        ShootProjectile();

        // Volver a la animación de Idle
        if (animator != null)
        {
            animator.SetTrigger("Idle"); // Activar el trigger
        }

        // Esperar el tiempo entre ataques
        yield return new WaitForSeconds(timeBetweenAttacks); // Resto del tiempo de espera

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