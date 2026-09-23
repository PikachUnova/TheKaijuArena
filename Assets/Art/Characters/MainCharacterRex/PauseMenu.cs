using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour

{
    public GameObject player;
    public UIHandler reset;
    GameObject[] pauseObjects;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pauseObjects = GameObject.FindGameObjectsWithTag("EditorOnly");
        HidePaused();  
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                ShowPaused();
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                HidePaused();
            }
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        HidePaused();
        Destroy(player);
    }

    public void PauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            ShowPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            HidePaused();
        }
    }

    public void ShowPaused()
    {
        foreach (GameObject g in pauseObjects)
            g.SetActive(true);
    }
    public void HidePaused()
    {
        foreach (GameObject g in pauseObjects)
            g.SetActive(false);
    }

}
