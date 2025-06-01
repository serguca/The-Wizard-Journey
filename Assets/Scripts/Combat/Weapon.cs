using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float damage = 10f; // Default damage value
    private Collider col;
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void Start()
    {
        col = GetComponent<Collider>();
        col.enabled = false;
    }

    private void Update()
    {
        Debug.Log($"[{col.enabled}]");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (damage > 0)
            {
                collision.GetComponent<Character>()?.TakeDamage(damage);
                Debug.Log($"Weapon: Evento enviado con daño: [{damage}]");
            }
            else Debug.LogError("Weapon: El daño es cero o negativo, no se envía el evento.");
        }
    }
    
    public void SetColliderActive(bool active)
    {
        if (col != null) col.enabled = active;
    }
}
