using UnityEngine;
using System.Collections;
using DialogueEditor;

public class BossDialogue : MonoBehaviour
{
    public NPCConversation conversationIntro;
    public NPCConversation conversationDefeat;

    private bool hasTriggeredDefeat = false;

    void Start()
    {
        StartCoversationIntro();
    }

    void Update()
    {
        if (hasTriggeredDefeat) return;

        if (this.GetComponent<EnemyHealth>() != null && this.GetComponent<EnemyHealth>().currentHealth <= 0)
        {
            StartCoversationDefeat();
            hasTriggeredDefeat = true;
        }
    }

    public void StartCoversationIntro()
    {
        ConversationManager.Instance.StartConversation(conversationIntro);
        if (AudioManager.audioManager != null)
            AudioManager.audioManager.PlayTrack(3);
        StartCoroutine(EndCoversationTime(4f));
    }
    public void StartCoversationDefeat()
    {
        ConversationManager.Instance.StartConversation(conversationDefeat);
        if (AudioManager.audioManager != null)
            AudioManager.audioManager.StopMusic();
        StartCoroutine(EndCoversationTime(2f));
    }

    private IEnumerator EndCoversationTime(float time)
    {
        yield return new WaitForSeconds(time);
        ConversationManager.Instance.EndConversation();
    }


}
