using System.Collections;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 10f;
    public float tiempoVida = 3f;
    private float tiempoTranscurrido = 0f;

    public GameObject explosionPrefab; // Prefab de la explosión

    public void Lanzar(Vector3 direccion)
    {
        Reiniciar(); // Reinicia el estado del proyectil
        // Reiniciar temporizador
        tiempoTranscurrido = 0f;

        // Activar el objeto
        gameObject.SetActive(true);

        // Configurar movimiento (usaremos Update para esto)
        // La dirección ya viene normalizada desde el HechizoManager
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
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f); // Escala más pequeña
                Destroy(explosion, 2f);
            }
            gameObject.SetActive(false);
        }
    }

    public void Reiniciar()
    {
        tiempoTranscurrido = 0f;
        StopAllCoroutines(); // Detener cualquier coroutine activa
        // Reinicia otros estados si es necesario
    }
}