using MagicPigGames;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected ProgressBar healthBar; // Referencia a la barra de salud
    protected float health;
    protected bool isDead = false;
    protected bool hitCooldownActive = false; // Bandera para evitar múltiples colisiones
    [SerializeField] protected float damage = 10f;
    [SerializeField] public AudioClip hitSound;
    [SerializeField] protected AudioClip attackSound;
    [SerializeField] protected AudioClip healSound; // Duración del cooldown de daño
    public float GetDamage()
    {
        return damage;
    }

    protected void SetProgressBar(float health)
    {
        if (health > 0)
            healthBar.SetProgress(health / maxHealth);
        else healthBar.SetProgress(0);
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log("Evento Recibido damage: " + damage);
        if (hitCooldownActive || isDead) return; // Evita daño si está en cooldown o muerto
        health -= damage;
        if (health < 0f) health = 0f; // Asegura que la salud no sea negativa
        if (health > maxHealth) health = maxHealth; // Asegura que la salud no supere el máximo
        SetProgressBar(health);

        if(damage > 0 && hitSound!=null) SoundManager.Instance.PlaySound(hitSound, transform.position, 0.5f);
        if(damage < 0 && healSound!=null) SoundManager.Instance.PlaySound(healSound, transform.position, 0.5f);

        if (health <= 0f && !isDead)
        {
            Die();
            return;
        }
        
        hitCooldownActive = true;
    }

    protected abstract void Die();
    
}