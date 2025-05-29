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

    public virtual void TakeDamage(float damage){}

}