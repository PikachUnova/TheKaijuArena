using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine.UI;
using TMPro;

public class CombatLevelSelector : MonoBehaviour
{
    public static CombatLevelSelector levelSelector;

    public NPCConversation conversation;

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
        ConversationManager.Instance.StartConversation(conversation);
        yield return new WaitForSeconds(2f);
        ConversationManager.Instance.EndConversation();

        CombatManager.combatManager.SetCombatLevel(data);
        CombatManager.combatManager.StartCombat();
        this.gameObject.SetActive(false);
    }

    public void UnlockLevel()
    {
        currentLevel++;
        Debug.Log("Unlocked Level " + currentLevel);
        buttons[(currentLevel - 1) % buttons.Length].interactable = true;
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }


}
