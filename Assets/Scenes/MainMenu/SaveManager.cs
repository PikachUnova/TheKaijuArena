using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance {get; set;}

    string jsonPathProject;
    string jsonPathPersistant;
    string binaryPath; 
    
    string fileName = "SaveGame";
    
    public bool isSavingJson;
    public bool isLoading;
    
    private void Awake() 
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //SaveData = new SaveData();
    }
    
    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
        binaryPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }
    
    public void SaveGame()
    {
    }
    /*
    private SaveData GetSaveData() 
    {	
        
        int currentLevel;
        int playerHealth;
        int playerMaxHealth;
        float playerX;
        float playerY;
        float playerZ;
        string sceneName;
            
        
        return new SaveData(playerStats, playerPosAndRot);
        
    }*/
    
    public void DeselectButton() 
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
}
