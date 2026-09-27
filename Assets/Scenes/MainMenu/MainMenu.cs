using UnityEngine;
using System.Collections;
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
        StartCoroutine(DelayPlay(3f));
    }

    private IEnumerator DelayPlay(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Loaded Game");
        SceneManager.LoadScene("Rex'sHouse");
    }

    public void PlayNewGame()
    {
        StartCoroutine(DelayPlayNewGame(3f));
    }

    private IEnumerator DelayPlayNewGame(float time)
    {
        yield return new WaitForSeconds(time);
        SaveManager.Instance.saveData.ResetSaveData();
        Debug.Log("New Game Started");
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
