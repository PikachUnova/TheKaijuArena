using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip[] menuClips;

    public AudioClip menuSoundtrack;

    private int slotNumber = 1;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(menuSoundtrack);
    }

    public void TempSaveGame()
    {
        //SceneManager.Instance.SaveGame(slotNumber);
    }


    public void Play()
    {
        SceneManager.LoadScene("Rex'sHouse");
    }

    public void OptionsGame()
    {
        SceneManager.LoadScene("Options");
    }

    public void HowToPlayGame()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void ExitGame()
	{
		Application.Quit();
	}

}
