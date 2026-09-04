using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;
    public AudioSource musicAudioSource;
    public AudioSource soundAudioSource;
    public AudioClip[] audioClips;


    public AudioClip[] musicTracks;

    void Start()
    {
        if (AudioManager.audioManager != null)
        {
            Destroy(this.gameObject);
            return;
        }
        audioManager = this;
        DontDestroyOnLoad(this);
        PlayTrack(1);
    }

    public void PlaySFX(int index, GameObject other)
    {
        soundAudioSource.PlayOneShot(audioClips[index]); // Loudness just right but sound far from listener should be more silent.
        //AudioSource.PlayClipAtPoint(audioClips[index], other.transform.position, 1f); // Does attentuation but too soft
    }

    void PlayTrack(int index)
    {
        if (index >= 0 && index < musicTracks.Length)
        {
            musicAudioSource.clip = musicTracks[index];
            musicAudioSource.Play();
        }
    }
}
