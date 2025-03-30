using System.Collections.Generic;
using UnityEngine;

public class HechizoManager : MonoBehaviour
{
    public GameObject proyectilPrefab;
    public int poolSize = 10;
    private List<GameObject> proyectilPool;

    void Start()
    {
        proyectilPool = new List<GameObject>();
        
        // Crear pool de proyectiles
        for (int i = 0; i < poolSize; i++)
        {
            GameObject proyectil = Instantiate(proyectilPrefab);
            proyectil.SetActive(false);
            proyectilPool.Add(proyectil);
        }
    }

public void LanzarProyectil(Vector3 posicion, Vector3 direccion)
{
    GameObject proyectil = proyectilPool.Find(p => !p.activeInHierarchy);
    
    if (proyectil != null)
    {
        proyectil.transform.position = posicion;
        proyectil.transform.rotation = Quaternion.LookRotation(direccion);
        proyectil.SetActive(true);
        
        Proyectil script = proyectil.GetComponent<Proyectil>();
        script.Lanzar(direccion); // Ahora pasamos la dirección
    }
}
}