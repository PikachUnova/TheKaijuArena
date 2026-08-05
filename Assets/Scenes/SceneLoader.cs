using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public SceneData sceneData;

    //Navigates the Player from one scene to another
    void OnTriggerEnter(Collider other)
    {   
        // The contacted object navigates the player 
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("KaijuPlaza");
        }
        //sceneData.sceneName
    }
    
}