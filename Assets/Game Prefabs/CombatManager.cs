using UnityEngine;
using System.Collections;

public class CombatManager : MonoBehaviour
{
    [Header("References")]
    private int level = 1;

    private int currentWave = 0;
    [SerializeField] private EnemySpawner enemySpawner;

    private bool hasStarted = false;
    private bool levelComplete = false;

    public void StartCombat()
    {
        StartCoroutine(StartCountDown()); // Start countdown
    }

    void Update()
    {
        if (!hasStarted)
            return;
        if (IsLevelComplete())
            return;

        CheckWaveComplete();
    }

    private IEnumerator StartCountDown()
    {
        Debug.Log("3");
        yield return new WaitForSeconds(1f);
        Debug.Log("2");
        yield return new WaitForSeconds(1f);
        Debug.Log("1");
        yield return new WaitForSeconds(1f);
        Debug.Log("GO!");
        hasStarted = true;
        StartNextWave();

    }

    private void StartNextWave()
    {
        Debug.Log("Wave " + (currentWave + 1) + " Start!");
        currentWave++;

        if (currentWave >= 5)
        {
            CompleteLevel();
            return;
        }

        //CombatLevelData.WaveData wave = combatLevelData.waves[currentWave];
        //enemySpawner.SpawnEnemies(wave.enemyPrefab, wave.enemyCount);
    }

    public void CheckWaveComplete()
    {

        Debug.Log("Wave " + currentWave + " Complete!");

        StartNextWave();
    }

    private void CompleteLevel()
    {
        levelComplete = true;
    }

    public int GetCurrentWave()
    {
        return currentWave + 1;
    }

    public bool IsLevelComplete()
    {
        return levelComplete;
    }

}
