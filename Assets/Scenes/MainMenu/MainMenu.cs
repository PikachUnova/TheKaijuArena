using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip menuSoundtrack;

    public GameObject loadGameButton;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(menuSoundtrack);

        // Check if there is saved game data
        if (SaveManager.Instance.saveData != null && SaveManager.Instance.saveData.HasSavedGame())
            loadGameButton.SetActive(true);
        else
            loadGameButton.SetActive(false);
        
    }

    public void LoadGame()
    {
        StartCoroutine(DelayPlay(3f, "MonsterPlaza"));
    }

    private IEnumerator DelayPlay(float time, string sceneName)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneName);
    }

    public void PlayNewGame()
    {
        if (SaveManager.Instance.saveData != null && SaveManager.Instance.saveData.HasSavedGame())
        {
            // Show a confirmation dialog to the player
            // If the player confirms, reset the save data and start a new game
            // If the player cancels, do nothing
            // For simplicity, we'll assume the player confirms here
            SaveManager.Instance.saveData.ResetSaveData();
        }
        StartCoroutine(DelayPlay(3f, "Rex'sHouse"));
    }

    public void ExitGame()
	{
		Application.Quit();
	}

}
