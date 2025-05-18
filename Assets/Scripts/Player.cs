using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float health = 100f; // Salud del jugador
    
     [SerializeField] private bool collided = false; // Bandera para evitar múltiples colisiones

    void Start(){}

    // Update is called once per frame
    void Update(){}

    void OnTriggerEnter(Collider other)
    {
        if(collided) return;
        if (other.CompareTag("Hit"))
        {
            health -= 10f;
            collided = true;
            Debug.Log("Player hit! Health: " + health);
            StartCoroutine(ResetCollided());
        }
    }

    private IEnumerator ResetCollided()
    {
        yield return new WaitForSeconds(0.5f);
        collided = false;
    }
}
