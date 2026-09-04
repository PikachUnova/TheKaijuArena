using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicVol, sfxVol;
    public void SetSFX(float volume)
    {
        audioMixer.SetFloat("soundVolume", volume);
        //PlayerPrefs.SetFloat("soundVolume", volume);
    }
    public void SetMusic(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
        //PlayerPrefs.SetFloat("musicVolume", volume);
    }
}
