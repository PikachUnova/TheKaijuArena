using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatLevelSelector : MonoBehaviour
{
    [SerializeField] private CombatLevelData levelData;
    [SerializeField] private Button button;
    [SerializeField] private CombatManager combatManager;

    public void SelectLevel()
    {
        //combatManager.SetCombatLevel(levelData);
    }

    public bool IsLevelUnlocked(int level)
    {
        if (level == 1)
            return true;

        return PlayerPrefs.GetInt("CombatLevel_" + level, 0) == 1;
    }

    public void UnlockLevel(int level)
    {
        PlayerPrefs.SetInt("CombatLevel_" + level, 1);
        PlayerPrefs.Save();
    }

}
