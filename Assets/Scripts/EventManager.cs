using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<float> DamagePlayer; // Evento para notificar el daño al jugador
                                                    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void TriggerDamagePlayer(float damage)
    {
        DamagePlayer?.Invoke(damage); // Invoca el evento si hay suscriptores
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
