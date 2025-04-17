using System.Collections.Generic;
using UnityEngine;

public class HechizoManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private GameObject explosionPrefab;

    [Header("Pool Settings")]
    [SerializeField] private int poolSize = 10;

    private readonly List<Proyectil> proyectilPool = new();
    private readonly List<Explosion> explosionPool = new();

    private void Awake()
    {
        InicializarPoolProyectiles();
        InicializarPoolExplosiones();
    }

    private void InicializarPoolProyectiles()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(proyectilPrefab);
            go.SetActive(false);

            Proyectil proyectil = go.GetComponent<Proyectil>();
            proyectilPool.Add(proyectil);

            // Inyectamos referencia del manager
            //proyectil.Inicializar(this);
        }
    }

    private void InicializarPoolExplosiones()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(explosionPrefab);
            go.SetActive(false);

            Explosion controller = go.GetComponent<Explosion>();
            explosionPool.Add(controller);
        }
    }

    public Proyectil ObtenerProyectil()
    {
        foreach (var p in proyectilPool)
        {
            if (!p.gameObject.activeInHierarchy)
                return p;
        }
        return null;
    }

    public Explosion ObtenerExplosion()
    {
        foreach (var e in explosionPool)
        {
            if (!e.gameObject.activeInHierarchy)
                return e;
        }
        return null;
    }

    public void LanzarProyectil(Vector3 posicion, Vector3 direccion)
    {
        Proyectil proyectil = ObtenerProyectil();

        if (proyectil != null)
        {
            GameObject go = proyectil.gameObject;
            go.transform.position = posicion;
            go.transform.rotation = Quaternion.LookRotation(direccion);
            go.SetActive(true);

            proyectil.Lanzar(direccion);
        }
    }
}
