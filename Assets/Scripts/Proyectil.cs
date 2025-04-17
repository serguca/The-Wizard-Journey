using System.Collections;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float tiempoVida = 3f;
    private float tiempoTranscurrido = 0f;

    private HechizoManager hechizoManager; // Referencia al HechizoManager

    void Start()
    {
        // Buscar el HechizoManager en la escena
        hechizoManager = FindFirstObjectByType<HechizoManager>();
    }

    public void Lanzar(Vector3 direccion)
    {
        Reiniciar(); // Reinicia el estado del proyectil
        tiempoTranscurrido = 0f;
        gameObject.SetActive(true);
        StartCoroutine(MoverProyectil(direccion));
    }

    IEnumerator MoverProyectil(Vector3 direccion)
    {
        while (tiempoTranscurrido < tiempoVida)
        {
            transform.position += direccion * velocidad * Time.deltaTime;
            tiempoTranscurrido += Time.deltaTime;
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
            if (hechizoManager != null)
            {
                // Obtener una explosión del pool
                GameObject explosion = hechizoManager.ObtenerExplosion();
                if (explosion != null)
                {
                    // Posicionar y activar la explosión
                    explosion.transform.position = transform.position;
                    explosion.transform.rotation = Quaternion.identity;
                    explosion.SetActive(true);

                    // Desactivar la explosión después de un tiempo
                    StartCoroutine(DesactivarExplosion(explosion));
                }
            }

            // Desactivar el proyectil
            gameObject.SetActive(false);
        }
    }

    IEnumerator DesactivarExplosion(GameObject explosion)
    {
        yield return new WaitForSeconds(2f); // Esperar 2 segundos
        explosion.SetActive(false); // Desactivar la explosión
    }

    public void Reiniciar()
    {
        tiempoTranscurrido = 0f;
        StopAllCoroutines(); // Detener cualquier coroutine activa
    }
}
