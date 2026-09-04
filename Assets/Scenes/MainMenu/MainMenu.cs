using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip[] menuClips;

    public AudioClip menuSoundtrack;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(menuSoundtrack);
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
