using System.Collections;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 10f;
    public float tiempoVida = 3f;
    private float tiempoTranscurrido = 0f;

    public void Lanzar(Vector3 direccion)
    {
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

    void OnTriggerEnter(Collider other)
    {
        // Aquí puedes añadir lógica de impacto
        if (!other.CompareTag("Player")) // Ejemplo: no colisionar con el jugador
        {
            // Efectos de impacto, daño, etc.
            gameObject.SetActive(false); // Devolver al pool
        }
    }
}