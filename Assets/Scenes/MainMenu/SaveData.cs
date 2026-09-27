[System.Serializable]
public class SaveData
{
    public int currentLevel;
    public int playerMaxHealth;

    public int playerAttackPower;
    public int playerSkills;


    public void ResetSaveData()
    {
        currentLevel = 0;
        playerMaxHealth = 100;
        playerAttackPower = 5;
        playerSkills = 0;
        
    }
}