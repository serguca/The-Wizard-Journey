// HechizoManager.cs
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject explotionPrefab;

    [Header("Pool Settings")]
    [SerializeField] private int poolSize = 10;

    private readonly List<Proyectile> projectilePool = new();
    private readonly List<Explotion> explotionPool = new();

    private void Awake()
    {
        InitializeProjectilePool();
        InitializeExplotionPool();
    }

    private void InitializeProjectilePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(projectilePrefab);
            go.SetActive(false);

            Proyectile projectile = go.GetComponent<Proyectile>();
            projectilePool.Add(projectile);

            // Inyectamos referencia del manager
            projectile.Initialize(this);
        }
    }

    private void InitializeExplotionPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(explotionPrefab);
            go.SetActive(false);

            Explotion controller = go.GetComponent<Explotion>();
            explotionPool.Add(controller);
        }
    }

    public Proyectile GetProjectile()
    {
        foreach (var p in projectilePool)
        {
            if (!p.gameObject.activeInHierarchy)
                return p;
        }
        return null;
    }

    public Explotion GetExplotion()
    {
        foreach (var e in explotionPool)
        {
            if (!e.gameObject.activeInHierarchy)
                return e;
        }
        return null;
    }

    public void LaunchProjectile(Vector3 position, Vector3 direction)
    {
        Proyectile projectile = GetProjectile();

        if (projectile != null)
        {
            GameObject go = projectile.gameObject;
            go.transform.position = position;
            go.transform.rotation = Quaternion.LookRotation(direction);
            go.SetActive(true);

            projectile.Launch(direction);
        }
    }
}