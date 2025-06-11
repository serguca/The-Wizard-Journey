using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Character
{
    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private SpellManager spellManager;
    [SerializeField] private Transform shootPoint;
    private GameObject deathScreen; 
    private GameObject winScreen; 
    private GameObject controlsScreen;
    [SerializeField] private float attackCooldown = 0.5f;
    private float lastShootTime = -999f;
    private Camera playerCamera;

    [Header("Damage Feedback")]
    [SerializeField] private Image damageFlashImage;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private Color flashColor = new Color(1f, 0f, 0f, 0.3f); // Rojo con 30% de opacidad

    [Header("Inventory")]
    [SerializeField] private Item smallHealthPotion;
    private bool isPlayingFootstep = false;
    private void Start()
    {
        spellManager.SetDamage(damage);
        FirstPersonController fpsController = GetComponent<FirstPersonController>();
        playerCamera = fpsController.playerCamera;

        health = maxHealth;
        deathScreen = GameObject.Find("DeathScreen");
        if (deathScreen != null) deathScreen.SetActive(false); // Oculta al inicio

        deathScreen = GameObject.Find("WinScreen");
        if (deathScreen != null) deathScreen.SetActive(false); // Oculta al inicio
        
        controlsScreen = GameObject.Find("ControlsScreen");

        if (damageFlashImage != null)
            damageFlashImage.enabled = false;
    }

    private void Update()
    {
        if (isDead) return;
        if (Input.GetMouseButtonDown(0) && Time.time >= lastShootTime + attackCooldown)
        {
            float shootPoint = 0.5f; //medio metro por delante de la cámara aparecerá el proyectil
            Vector3 shootPosition = playerCamera.transform.position + playerCamera.transform.forward * shootPoint;
            spellManager.LaunchProjectile(shootPosition, playerCamera.transform.forward, damage, this.tag);
            lastShootTime = Time.time;
            if (attackSound != null)
            {
                SoundManager.Instance.PlaySound(attackSound, shootPosition);
            }
        }
        if (Input.GetKeyDown(KeyCode.F) && Inventory.Instance.HasItem(smallHealthPotion))
        {
            Inventory.Instance.RemoveItem(smallHealthPotion);
            TakeDamage(-50f);

            Debug.Log("Usando poción de salud: " + smallHealthPotion);
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (controlsScreen != null)
            {
                controlsScreen.SetActive(!controlsScreen.activeSelf);
            }
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
        base.TakeDamage(damage);
        StartCoroutine(DamageFlashEffect());
        StartCoroutine(ColliderCooldown());
    }

    private IEnumerator DamageFlashEffect()
    {
        if (damageFlashImage != null)
        {
            float elapsedTime = 0f;
            float startAlpha = 0f;
            float maxAlpha = flashColor.a;

            // FADE IN
            damageFlashImage.enabled = true;
            while (elapsedTime < flashDuration / 2f)
            {
                elapsedTime += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(startAlpha, maxAlpha, elapsedTime / (flashDuration / 2f));
                damageFlashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, currentAlpha);
                yield return null;
            }

            // FADE OUT
            elapsedTime = 0f;
            while (elapsedTime < flashDuration / 2f)
            {
                elapsedTime += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(maxAlpha, 0f, elapsedTime / (flashDuration / 2f));
                damageFlashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, currentAlpha);
                yield return null;
            }

            damageFlashImage.enabled = false;
        }
    }

    protected override void Die()
    {
        isDead = true;
        if (deathScreen != null) deathScreen.SetActive(true);
        GetComponent<FirstPersonController>().enabled = false;
        // transform.position = deathRoom.position; // Teletransporta al jugador a la habitación de muerte
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

    public void PlayFootstepSound()
    {
        if (footstepSound != null && !isPlayingFootstep)
        {
            StartCoroutine(PlayFootstepCoroutine());
        }
    }
    
    private IEnumerator PlayFootstepCoroutine()
    {
        isPlayingFootstep = true;
        
        // Reproduce el sonido
        SoundManager.Instance.PlaySound(footstepSound, transform.position, 1f);
        
        // Espera la duración del clip
        yield return new WaitForSeconds(footstepSound.length);
        
        isPlayingFootstep = false;
    }
}
