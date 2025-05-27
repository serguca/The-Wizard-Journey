using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<float> DamagePlayer;
    public static event Action<float> DamageEnemy;

    public static event Action ActivateWeapon;

    public static event Action PlayerDied; //todo: implementar

    public static void TriggerDamagePlayer(float damage)
    {
        DamagePlayer?.Invoke(damage);
    }

    public static void TriggerDamageEnemy(float damage)
    {
        DamageEnemy?.Invoke(damage);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
