using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CombatLevelSelector : MonoBehaviour
{
    public static CombatLevelSelector levelSelector;

    private int currentLevel = 0;

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
        DontDestroyOnLoad(this);

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].interactable = false;
        UnlockLevel();
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
            currentLevel++;
        buttons[(currentLevel - 1) % buttons.Length].interactable = true;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }


}
