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

    public void SelectLevel(CombatLevelData data)
    {
        StartCoroutine(StartLevel(data));
    }

    private IEnumerator StartLevel(CombatLevelData data)
    {
        ConversationManager.Instance.StartConversation(conversation);
        yield return new WaitForSeconds(2f);
        ConversationManager.Instance.EndConversation();

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
