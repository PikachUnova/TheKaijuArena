using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DialogueEditor;
using TMPro;

public class CombatManager : MonoBehaviour
{
    [Header("References")]
    public static CombatManager combatManager;
    private GameObject player;
    [SerializeField] private EnemySpawner enemySpawner;
    public TMP_Text combatText;


    [Header("Position Points")]
    public Transform startingPosition;
    public Transform npcPosition;


    [Header("Combat Level")]
    private CombatLevelData combatLevelData;

    private int currentWave = 0;

    private bool hasStarted = false;
    private bool levelComplete = false;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (CombatManager.combatManager != null)
        {
            Destroy(this.gameObject);
            return;
        }
        combatManager = this;
        DontDestroyOnLoad(this);
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
            
        if (player.GetComponent<PlayerHealth>().IsDefeated())
        {
            player.GetComponent<PlayerHealth>().Respawn();
            currentWave = 0;
            hasStarted = false;
            levelComplete = false;
            combatText.text = "";
            enemySpawner.ClearEnemies();
        }
        else if (IsLevelComplete())
        {
            currentWave = 0;
            hasStarted = false;
            levelComplete = false;
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
        AudioManager.audioManager.StopMusic();
        yield return new WaitForSeconds(2f);
        AudioManager.audioManager.PlaySFX(4);
        combatText.text = "3";
        yield return new WaitForSeconds(1f);
        AudioManager.audioManager.PlaySFX(4);
        combatText.text = "2";
        yield return new WaitForSeconds(1f);
        AudioManager.audioManager.PlaySFX(4);
        combatText.text = "1";
        yield return new WaitForSeconds(1f);
        AudioManager.audioManager.PlaySFX(4);
        combatText.text = "GO!";
        hasStarted = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        StartNextWave();
        yield return new WaitForSeconds(1f);
        combatText.text = "";
        AudioManager.audioManager.PlayTrack(2);

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
        if (combatLevelData.level >= CombatLevelSelector.levelSelector.GetCurrentLevel()) // Do not unlock if already done
            CombatLevelSelector.levelSelector.UnlockLevel();
        
        AudioManager.audioManager.StopMusic();
        AudioManager.audioManager.PlaySFX(5);

        yield return new WaitForSeconds(2f);
        combatText.text = "";
        UIHandler.handler.FadeOut();
        yield return new WaitForSeconds(0.7f);
        SetPlayerLocation(npcPosition);
        yield return new WaitForSeconds(1f);
        UIHandler.handler.FadeIn();
        yield return new WaitForSeconds(0.5f);

        AudioManager.audioManager.PlayTrack(1);

        GameObject npc = GameObject.FindGameObjectWithTag("NPC");
        npc.GetComponent<NPCInteractable>().StartCoversationWin();
    }

}
