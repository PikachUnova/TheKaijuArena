using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DialogueEditor;
using TMPro;

public class CombatManager : MonoBehaviour
{
    [Header("References")]
    private GameObject player;
    [SerializeField] private EnemySpawner enemySpawner;
    public TMP_Text combatText;

    public NPCConversation conversation;


    [Header("Position Points")]
    public Transform startingPosition;
    public Transform npcPosition;


    [Header("Combat Level")]
    private CombatLevelData combatLevelData;

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

    public void SetCombatLevel(CombatLevelData data)
    {
        combatLevelData = data;
    }

    void Update()
    {
        if (!hasStarted) // Don't do anything if not started yet
            return;
        
        if (IsLevelComplete())
        {
            combatText.text = "Level Cleared!";
            combatText.fontSize = 72;
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
        combatText.text = "3";
        yield return new WaitForSeconds(1f);
        combatText.text = "2";
        yield return new WaitForSeconds(1f);
        combatText.text = "1";
        yield return new WaitForSeconds(1f);
        combatText.text = "GO!";
        hasStarted = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        StartNextWave();
        yield return new WaitForSeconds(1f);
        combatText.text = "";

    }

    private IEnumerator DisplayText(string text)
    {
        combatText.text = text;
        yield return new WaitForSeconds(2f);
        combatText.text = "";
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
        enemySpawner.SpawnEnemies(combatLevelData.waves[currentWave - 1].enemyPrefabs, combatLevelData.waves[currentWave - 1].enemyCount);
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
        yield return new WaitForSeconds(2f);
        UIHandler.handler.FadeOut();

        combatText.text = "";
        currentWave = 0;
        hasStarted = false;
        levelComplete = false;

        yield return new WaitForSeconds(0.5f);
        SetPlayerLocation(npcPosition);

        yield return new WaitForSeconds(1.2f);
        UIHandler.handler.FadeIn();
        yield return new WaitForSeconds(0.5f);
        ConversationManager.Instance.StartConversation(conversation);
    }

}
