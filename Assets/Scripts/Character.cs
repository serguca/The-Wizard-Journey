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

    protected virtual void Start()
    {
    }
}