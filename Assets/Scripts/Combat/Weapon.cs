using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float damage = 0; // Default damage value
    private Collider[] colliders;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void Start()
    {
        // Obtén todos los colliders en este GameObject
        colliders = GetComponents<Collider>();
        // Desactiva todos al inicio
        foreach (var c in colliders)
            c.enabled = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (damage <= 0) Debug.Log("No has seteado el daño de la Weapon");
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Character>()?.TakeDamage(damage);
        }
    }
    
    public void SetColliderActive(bool active)
    {
        if (colliders != null && colliders.Length > 0)
        {
            foreach (var c in colliders)
                c.enabled = active;
        }
    }
}