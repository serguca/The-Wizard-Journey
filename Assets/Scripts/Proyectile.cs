// Proyectil.cs
using System.Collections;
using UnityEngine;

public class Proyectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    private float elapsedTime = 0f;
    private float damage;

    private SpellManager spellManager; // Referencia al HechizoManager

    public void Initialize(SpellManager manager, float damage)
    {
        this.damage = damage; // Guardar el daño del proyectil
        spellManager = manager; // Cachear la referencia al HechizoManager
    }

    public void Launch(Vector3 direction)
    {
        Reset(); // Reinicia el estado del proyectil
        elapsedTime = 0f;
        gameObject.SetActive(true);
        StartCoroutine(MoveProjectile(direction));
    }

    private IEnumerator MoveProjectile(Vector3 direction)
    {
        while (elapsedTime < lifetime)
        {
            transform.position += direction * speed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Desactivar el proyectil cuando termine su tiempo de vida
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        MakeExplosion();
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Debug.Log("Proyectile: golpea al jugador");
            other.GetComponent<Character>()?.TakeDamage(damage);
        }

        gameObject.SetActive(false);
    }

    private void MakeExplosion()
    {
        if (spellManager != null)
        {
            Explotion explotion = spellManager.GetExplotion();
            if (explotion != null)
            {
                explotion.transform.position = transform.position;
                explotion.gameObject.SetActive(true);

                explotion.Activate(2f); // Activar la explosión por 2 segundos
            }
        }
    }

    private void Reset()
    {
        elapsedTime = 0f;
        StopAllCoroutines(); // Detener cualquier coroutine activa
    }
}