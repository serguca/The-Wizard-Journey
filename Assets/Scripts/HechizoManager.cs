using System.Collections.Generic;
using UnityEngine;

public class HechizoManager : MonoBehaviour
{
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private GameObject explosionPrefab; // Prefab de la explosión
    [SerializeField] private int poolSize = 10;
    private List<GameObject> proyectilPool;
    private List<GameObject> explosionPool;

    void Start()
    {
        // Crear pool de proyectiles
        proyectilPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject proyectil = Instantiate(proyectilPrefab);
            proyectil.SetActive(false);
            proyectilPool.Add(proyectil);
        }

        // Crear pool de explosiones
        explosionPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.SetActive(false);
            explosionPool.Add(explosion);
        }
    }

    public GameObject ObtenerProyectil()
    {
        return proyectilPool.Find(p => !p.activeInHierarchy);
    }

    public GameObject ObtenerExplosion()
    {
        return explosionPool.Find(e => !e.activeInHierarchy);
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