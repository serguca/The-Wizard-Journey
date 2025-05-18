using System.Collections;
using MagicPigGames;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float health = 100f; // Salud del jugador
    
    [SerializeField] private bool hitCooldownActive = false; // Bandera para evitar múltiples colisiones

    [SerializeField] private VerticalProgressBar healthBar; // Referencia a la barra de salud

    void Start(){}

    // Update is called once per frame
    void Update(){
        healthBar.SetProgress(health / 100f); // Actualiza la barra de salud
    }

    void OnTriggerEnter(Collider other)
    {
        if(hitCooldownActive) return;
        if (other.CompareTag("Hit"))
        {
            health -= 10f;
            hitCooldownActive = true;
            Debug.Log("Player hit! Health: " + health);
            StartCoroutine(ColliderCooldown());
        }
    }

    private IEnumerator ColliderCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        hitCooldownActive = false;
    }
}
