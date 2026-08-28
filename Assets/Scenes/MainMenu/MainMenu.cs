using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    

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
