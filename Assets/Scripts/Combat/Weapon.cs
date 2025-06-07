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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Character>()?.TakeDamage(damage);
        }
    }
    
    public void SetColliderActive(bool active)
    {
        if (col != null) col.enabled = active;
    }
}
