using UnityEngine;

public class HechizoVarita : MonoBehaviour
{
    public float distanciaMaxima = 100f;
    public LayerMask capasColision;
    public ParticleSystem sistemaParticulas;
    public float velocidadHechizo = 30f;
    
    private bool lanzandoHechizo = false;
    private Vector3 direccionHechizo;
    private Vector3 posicionInicial;
    private float distanciaRecorrida = 0f;
    
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Cambia esto por tu input
        {
            LanzarHechizo();
        }
        
        if (lanzandoHechizo)
        {
            ActualizarHechizo();
        }
    }
    
    void LanzarHechizo()
    {
        lanzandoHechizo = true;
        direccionHechizo = transform.forward;
        posicionInicial = transform.position;
        distanciaRecorrida = 0f;
        
        // Activar sistema de partículas
        if (sistemaParticulas != null)
        {
            sistemaParticulas.Clear();
            sistemaParticulas.Play();
        }
    }
    
    void ActualizarHechizo()
    {
        // Calcular nueva posición
        float distanciaFrame = velocidadHechizo * Time.deltaTime;
        distanciaRecorrida += distanciaFrame;
        Vector3 nuevaPosicion = posicionInicial + direccionHechizo * distanciaRecorrida;
        
        // Mover el sistema de partículas
        if (sistemaParticulas != null)
        {
            sistemaParticulas.transform.position = nuevaPosicion;
        }
        
        // Verificar colisión
        RaycastHit hit;
        if (Physics.Raycast(posicionInicial, direccionHechizo, out hit, distanciaRecorrida, capasColision))
        {
            ImpactoHechizo(hit);
        }
        
        // Verificar distancia máxima
        if (distanciaRecorrida >= distanciaMaxima)
        {
            TerminarHechizo();
        }
    }
    
    void ImpactoHechizo(RaycastHit hit)
    {
        Debug.Log("Hechizo impactó con: " + hit.collider.name);
        
        // Aquí puedes añadir efectos de impacto, daño, etc.
        
        TerminarHechizo();
    }
    
    void TerminarHechizo()
    {
        lanzandoHechizo = false;
        
        // Detener partículas
        if (sistemaParticulas != null)
        {
            sistemaParticulas.Stop();
        }
    }
}