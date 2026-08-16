using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public void SpawnEnemies(GameObject enemyPrefab, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];

            GameObject enemy = Instantiate(
                enemyPrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            spawnedEnemies.Add(enemy);

        }
    }

    private void EnemyDied(EnemyHealth enemyHealth)
    {
        GameObject enemy = enemyHealth.gameObject;

        if (spawnedEnemies.Contains(enemy))
        {
            spawnedEnemies.Remove(enemy);
        }
    }

    public int GetAliveEnemyCount()
    {
        return spawnedEnemies.Count;
    }

    public void ClearEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }

        spawnedEnemies.Clear();
    }
    
}
