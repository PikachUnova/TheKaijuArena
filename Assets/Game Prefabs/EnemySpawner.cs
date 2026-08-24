using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    public void SpawnEnemies(GameObject [] enemyPrefabs, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            Instantiate(enemyPrefabs[i%enemyPrefabs.Length], spawnPoint.position, spawnPoint.rotation);
        }
    }

    public int GetAliveEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    
}
