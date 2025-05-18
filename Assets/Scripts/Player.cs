using System;
using System.Collections;
using MagicPigGames;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Salud del jugador
    private float health = 100f; // Salud del jugador
    
    [SerializeField] private bool hitCooldownActive = false; // Bandera para evitar múltiples colisiones

    [SerializeField] private VerticalProgressBar healthBar; // Referencia a la barra de salud

    [SerializeField] private SpellManager spellManager;
    [SerializeField] private Transform shootPoint;

    private bool isDead = false;

    private GameObject deathScreen;


    void Start(){
        health = maxHealth;
        deathScreen = GameObject.Find("DeathScreen"); // Asegúrate de que el nombre coincida exactamente
        if (deathScreen != null) deathScreen.SetActive(false); // Oculta al inicio
    }

    // Update is called once per frame
    void Update(){
        //healthBar.SetProgress(health / 100f); // Actualiza la barra de salud
        if(isDead) return; // Bloquea toda la lógica si está muerto
        if (Input.GetMouseButtonDown(0))
        {
            spellManager.LaunchProjectile(shootPoint.position, shootPoint.forward);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(hitCooldownActive) return;
        if (other.CompareTag("Hit"))
        {
            TakeDamage(10);
            Debug.Log("Player hit! Health: " + health);
            StartCoroutine(ColliderCooldown());
        }
    }

    private IEnumerator ColliderCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        hitCooldownActive = false;
    }

    private void TakeDamage(float damage){
        hitCooldownActive = true;
        StartCoroutine(ColliderCooldown());
        health -= damage;
        healthBar.SetProgress(health / maxHealth); // Actualiza la barra de salud
        if (health <= 0f && !isDead) Die();
    }


    private void Die()
    {
        isDead = true;

            if (deathScreen != null)
        deathScreen.SetActive(true);
    }

}
