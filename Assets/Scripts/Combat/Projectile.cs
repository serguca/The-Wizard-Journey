// Proyectil.cs
using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private AudioClip hitSound; // Prefab de explosión, si es necesario
    private float elapsedTime = 0f;
    private float damage;

    private string whoFired;
    private SpellManager spellManager; // Referencia al HechizoManager
    private string owner;

    public void Initialize(SpellManager manager, float damage, string owner)
    {
        this.damage = damage; // Guardar el daño del proyectil
        spellManager = manager; // Cachear la referencia al HechizoManager
        this.owner = owner; // Guarda el owner
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
        Debug.Log($"Projectile: colisiona con {other.name}");
        if (other.CompareTag(owner)) return; // No dañes al que disparó
        MakeExplosion();
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Debug.Log("Projectile: golpea al jugador");
            Character character = other.GetComponent<Character>();
            if (character == null)
            {
                Debug.Log("Es null :(");
                character = other.GetComponentInParent<Character>();
            }
            character?.TakeDamage(damage);
            if (character == null) Debug.Log("Sigue siendo null :(");
        }
        else
        {
            AudioManager.Instance.PlaySound(hitSound, transform.position);
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