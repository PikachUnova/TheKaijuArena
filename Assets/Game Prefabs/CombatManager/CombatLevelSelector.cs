using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CombatLevelSelector : MonoBehaviour
{
    public static CombatLevelSelector levelSelector;
    public CharacterStats stats;
    
    private int currentLevel;

    public Button[] buttons;


    void Start()
    {
        this.gameObject.SetActive(false);

        if (CombatLevelSelector.levelSelector != null)
        {
            Destroy(this.gameObject);
            return;
        }
        levelSelector = this;

        currentLevel = SaveManager.Instance.saveData.currentLevel;
        for (int i = currentLevel + 1; i < buttons.Length; i++)
            buttons[i].interactable = false;
    }

    public void SelectLevel(CombatLevelData data)
    {
        StartCoroutine(StartLevel(data));
    }

    private IEnumerator StartLevel(CombatLevelData data)
    {
        GameObject npc = GameObject.FindWithTag("NPC");
        npc.GetComponent<NPCInteractable>().StartConversationPrepareBattle(true);
        yield return new WaitForSeconds(2f);
        npc.GetComponent<NPCInteractable>().StartConversationPrepareBattle(false);

        CombatManager.combatManager.SetCombatLevel(data);
        CombatManager.combatManager.StartCombat();
        this.gameObject.SetActive(false);
    }

    public void UnlockLevel()
    {
        if (currentLevel <= 10) // Up to a Max Level
        {
            currentLevel++;
            SaveManager.Instance.saveData.currentLevel++;
        }
        buttons[currentLevel % buttons.Length].interactable = true;

        if(currentLevel == 2 || currentLevel == 4 || currentLevel == 6)
        {
            stats.skills++;
            SaveManager.Instance.saveData.playerSkills++;
            Debug.Log("New Skill");
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }


}
