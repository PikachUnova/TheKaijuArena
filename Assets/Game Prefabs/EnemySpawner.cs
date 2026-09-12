using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public static EnemySpawner Instance { get; private set; }
    [SerializeField] private Transform[] spawnPoints;
    public int enemiesLeft = 0;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SpawnEnemies(GameObject [] enemyPrefabs)
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            Instantiate(enemyPrefabs[i%enemyPrefabs.Length], spawnPoint.position, spawnPoint.rotation);
            enemiesLeft++;
        }
    }

    public void DecrementEnemy()
    {
        enemiesLeft--;
    }

    public int GetAliveEnemyCount()
    {
        return enemiesLeft;
    }

    public void ClearEnemies()
    {
        StartCoroutine(Clear());
    }

    private IEnumerator Clear()
    {
        yield return new WaitForSeconds(2.5f);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
    
}
