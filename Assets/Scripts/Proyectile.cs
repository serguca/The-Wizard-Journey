// Proyectil.cs
using System.Collections;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    private float elapsedTime = 0f;

    private SpellManager spellManager; // Referencia al HechizoManager

    void Start()
    {
        // Buscar el HechizoManager en la escena
        spellManager = FindFirstObjectByType<SpellManager>();
    }

    public void Initialize(SpellManager manager)
    {
        spellManager = manager; // Cachear la referencia al HechizoManager
    }

    public void Launch(Vector3 direction)
    {
        Reset(); // Reinicia el estado del proyectil
        elapsedTime = 0f;
        gameObject.SetActive(true);
        StartCoroutine(MoveProjectile(direction));
    }

    IEnumerator MoveProjectile(Vector3 direction)
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

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Colisión detectada con: {collision.gameObject.name}");
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (spellManager != null)
            {
                // Obtener una explosión del pool
                Explotion explotion = spellManager.GetExplotion();
                if (explotion != null)
                {
                    // Posicionar y activar la explosión
                    explotion.transform.position = transform.position;
                    explotion.gameObject.SetActive(true);

                    // Activar la lógica de la explosión
                    explotion.Activate(2f); // Activar la explosión por 2 segundos
                }
            }

            // Desactivar el proyectil
            gameObject.SetActive(false);
        }
    }

    public void Reset()
    {
        elapsedTime = 0f;
        StopAllCoroutines(); // Detener cualquier coroutine activa
    }
}