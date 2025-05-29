using System;
using System.Collections;
using MagicPigGames;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Character
{
    [SerializeField] private SpellManager spellManager;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform deathRoom;
    private GameObject deathScreen; //todo: hacerlo con eventos

    [SerializeField] private float attackCooldown = 0.25f; // Tiempo de cooldown entre disparos
    private float lastShootTime = -999f;
    private void Start()
    {
        spellManager.SetDamage(damage);
        Component FPSController = GetComponent<FirstPersonController>();
        health = maxHealth;
        deathScreen = GameObject.Find("DeathScreen"); // Asegúrate de que el nombre coincida exactamente
        if (deathScreen != null) deathScreen.SetActive(false); // Oculta al inicio
    }

    // Update is called once per frame
    private void Update()
    {
        if (isDead) return; // Bloquea toda la lógica si está muerto
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShootTime + attackCooldown)
        {
            spellManager.LaunchProjectile(shootPoint.position, shootPoint.forward, damage);
            lastShootTime = Time.time;
        }
    }

    // private void OnEnable()
    // {
    //     EventManager.DamagePlayer += TakeDamage; // Suscribirse al evento de daño    
    // }

    //     private void OnDisable()
    // {
    //     EventManager.DamagePlayer -= TakeDamage; // Suscribirse al evento de daño    
    // }


    private void OnTriggerEnter(Collider other)
    {
        if (!hitCooldownActive) StartCoroutine(ColliderCooldown());
    }

    private IEnumerator ColliderCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        hitCooldownActive = false;
    }

    override public void TakeDamage(float damage)
    {
        Debug.Log("Evento Recibido damage: " + damage);
        if (hitCooldownActive || isDead) return; // Evita daño si está en cooldown o muerto
        health -= damage;
        SetProgressBar(health);
        if (health <= 0f && !isDead)
        {
            Die();
            return;
        }
        hitCooldownActive = true;
        StartCoroutine(ColliderCooldown());
    }


    private void Die()
    {
        isDead = true;
        if (deathScreen != null) deathScreen.SetActive(true);
        GetComponent<FirstPersonController>().enabled = false;
        transform.position = deathRoom.position; // Teletransporta al jugador a la habitación de muerte
        StartCoroutine(WaitForKeyPressAndRestart());
    }

    private IEnumerator WaitForKeyPressAndRestart()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            yield return null; // Espera un frame antes de volver a comprobar
        }
    }
}
