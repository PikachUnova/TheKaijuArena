using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine.UI;
using TMPro;

public class CombatLevelSelector : MonoBehaviour
{
    public static CombatLevelSelector levelSelector;

    [SerializeField] private CombatManager combatManager;
    public NPCConversation conversation;

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
    }

    void Update()
    {
        //if (ConversationManager.Instance.IsConversationActive)
    }

    public void SelectLevel(CombatLevelData data)
    {
        ConversationManager.Instance.StartConversation(conversation);
        combatManager.SetCombatLevel(data);
        combatManager.StartCombat();
        this.gameObject.SetActive(false);
    }

    public void UnlockLevel(int level)
    {
        PlayerPrefs.SetInt("CombatLevel_" + level, 1);
        PlayerPrefs.Save();
    }


}
