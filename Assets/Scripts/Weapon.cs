using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] private Enemy enemy;
    private float damage;
    void Start()
    {
        damage = enemy.GetDamage();
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
