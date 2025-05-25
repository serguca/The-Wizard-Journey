using UnityEngine;

public abstract class Character : MonoBehaviour
{
    // [SerializeField] protected float maxHealth = 100f;
    // protected float health;
    [SerializeField] protected float damage = 10f;

    public float GetDamage()
    {
        return damage;
    }

    // public virtual void TakeDamage(float amount)
    // {
    //     health -= amount;
    //     if (health < 0) health = 0;
    //     // Aquí puedes poner lógica común, como morir, actualizar UI, etc.
    // }

        protected virtual void Start()
    {
        Debug.Log("A");
    }
}