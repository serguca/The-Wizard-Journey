using System.Collections;
using UnityEngine;

public class VictoryChecker : MonoBehaviour
{
    [SerializeField] private float checkInterval = 5f;
    [SerializeField] private float detectionRadius = 10f; // Radio para buscar enemigos
    private bool bossDefeated = false;
    private Player player;
    
    private void Start()
    {
        // Obtener referencia al player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }
    
    // Método público para activar el checker cuando el boss muere
    public void StartVictoryCheck()
    {
        if (!bossDefeated)
        {
            bossDefeated = true;
            Debug.Log("Boss defeated! Starting victory check every 5 seconds...");
            StartCoroutine(CheckForRemainingEnemies());
        }
    }
    
    private IEnumerator CheckForRemainingEnemies()
    {
        while (bossDefeated)
        {
            yield return new WaitForSeconds(checkInterval);
            
            // Buscar todos los GameObjects con tag "Enemy"
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            
            // Filtrar solo los que están activos y son arañas
            int activeSpiders = 0;
            foreach (GameObject enemy in enemies)
            {
                if (enemy.activeInHierarchy)
                {
                    // Verificar si es una araña (tiene componente SpiderEnemy)
                    SpiderEnemy spider = enemy.GetComponent<SpiderEnemy>();
                    if (spider != null && !spider.isDead)
                    {
                        activeSpiders++;
                    }
                }
            }
            
            Debug.Log($"Active spiders remaining: {activeSpiders}");
            
            // Si no quedan arañas activas, ganar el juego
            if (activeSpiders == 0)
            {
                Debug.Log("No more spiders detected! Victory!");
                if (player != null)
                {
                    player.SetWin();
                }
                yield break; // Salir del bucle
            }
        }
    }
    
    // Método alternativo usando detección por radio
    private IEnumerator CheckForRemainingEnemiesInRadius()
    {
        while (bossDefeated)
        {
            yield return new WaitForSeconds(checkInterval);
            
            // Buscar enemigos en un radio específico
            Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius);
            
            int activeSpiders = 0;
            foreach (Collider col in enemiesInRange)
            {
                if (col.CompareTag("Enemy"))
                {
                    SpiderEnemy spider = col.GetComponent<SpiderEnemy>();
                    if (spider != null && !spider.isDead)
                    {
                        activeSpiders++;
                    }
                }
            }
            
            Debug.Log($"Active spiders in radius: {activeSpiders}");
            
            if (activeSpiders == 0)
            {
                Debug.Log("No more spiders in area! Victory!");
                if (player != null)
                {
                    player.SetWin();
                }
                yield break;
            }
        }
    }
    
    // Método para detener el checker si es necesario
    public void StopVictoryCheck()
    {
        bossDefeated = false;
        StopAllCoroutines();
    }
    
    // Visualizar el radio de detección en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}