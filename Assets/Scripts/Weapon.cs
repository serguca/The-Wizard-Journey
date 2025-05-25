using System;
using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private Character character;
    private float damage;
    void Start()
    {
        damage = character.GetDamage();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Weapon: Evento enviado");
            EventManager.TriggerDamagePlayer(damage);
        }
    }
}
