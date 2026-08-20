using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class CombatManager : MonoBehaviour
{
    [Header("References")]
    private GameObject player;
    [SerializeField] private EnemySpawner enemySpawner;
    public TMP_Text countDownText;


    [Header("Position Points")]
    public Transform startingPosition;
    public Transform npcPosition;


    [Header("Combat Level")]
    [SerializeField] private CombatLevelData combatLevelData;

    private int level = 1;

    private int currentWave = 0;

    private bool hasStarted = false;
    private bool levelComplete = false;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartCombat()
    {
        StartCoroutine(StartCountDown()); // Start countdown
    }

    void Update()
    {
        if (!hasStarted)
            return;
        if (IsLevelComplete())
        {
            countDownText.text = "Level Cleared!";
            countDownText.fontSize = 72;
            StartCoroutine(EndCombat());
            return;
        }

        CheckWaveComplete();
    }

    private void SetPlayerLocation(Transform point)
    {
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            player.transform.position = point.position;
            controller.enabled = true;
        }
    }

    private IEnumerator StartCountDown()
    {
        SetPlayerLocation(startingPosition);
        player.GetComponent<PlayerMovement>().enabled = false;
        yield return new WaitForSeconds(2f);
        countDownText.text = "3";
        yield return new WaitForSeconds(1f);
        countDownText.text = "2";
        yield return new WaitForSeconds(1f);
        countDownText.text = "1";
        yield return new WaitForSeconds(1f);
        countDownText.text = "GO!";
        hasStarted = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        StartNextWave();
        yield return new WaitForSeconds(1f);
        countDownText.text = "";

    }

    private IEnumerator DisplayText(string text)
    {
        countDownText.text = text;
        yield return new WaitForSeconds(2f);
        countDownText.text = "";
    }

    private void StartNextWave()
    {
        if (currentWave >= 5)
        {
            CompleteLevel();
            return;
        }
        currentWave++;
        if (currentWave != 1)
            StartCoroutine(DisplayText("Wave " + currentWave));
        enemySpawner.SpawnEnemies(combatLevelData.waves[currentWave - 1].enemyCount);
    }

    public void CheckWaveComplete()
    {
        if (enemySpawner.GetAliveEnemyCount() <= 0)
        {
            StartCoroutine(DisplayText("Wave " + currentWave + " Complete!"));
            StartNextWave();
        }
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

    private IEnumerator EndCombat()
    {
        yield return new WaitForSeconds(3f);
        SetPlayerLocation(npcPosition);
        countDownText.text = "";
        currentWave = 0;
        hasStarted = false;
        levelComplete = false;
        this.enabled = false;
    }

}
