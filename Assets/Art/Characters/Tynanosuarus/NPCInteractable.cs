using UnityEngine;
using System.Collections;
using DialogueEditor;

public class NPCInteractable : MonoBehaviour
{
    private GameObject player;

    public NPCConversation conversation;
    public NPCConversation conversationStartBattle;
    public NPCConversation conversationWin;
    public NPCConversation conversationLoss;

    private bool isTalking = false;
    [SerializeField] private float turnSpeed = 180f; // degrees per second
    [SerializeField] private float facingThreshold = 90f; // degrees

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(isTalking)
            player.GetComponent<PlayerMovement>().SetLocomotive(0f);
    }

    public void Speak()
    {
        if (isTalking || ConversationManager.Instance.IsConversationActive)
            return;
        isTalking = true;
        player.GetComponent<PlayerMovement>().enabled = false;
        StartCoroutine(Turn());
    }

    IEnumerator Turn()
    {
        while (!IsFacingPlayer())
        {
            LookAtPlayer();
            yield return null;
        }
        ConversationManager.Instance.StartConversation(conversation);
    }

    bool IsFacingPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float angle = Quaternion.Angle(transform.rotation, targetRotation);

        return angle < facingThreshold;
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    public void StartConversationWin()
    {
        ConversationManager.Instance.StartConversation(conversationWin);
        isTalking = true;
        player.GetComponent<PlayerMovement>().enabled = false;
    }
    public void StartConversationLoss()
    {
        ConversationManager.Instance.StartConversation(conversationLoss);
        isTalking = true;
        player.GetComponent<PlayerMovement>().enabled = false;
    }

    public void StartConversationPrepareBattle(bool b)
    {
        if (b)
        {
            ConversationManager.Instance.StartConversation(conversationStartBattle);
            isTalking = true;
            player.GetComponent<PlayerMovement>().enabled = false;
        }
        else
            ConversationManager.Instance.EndConversation();
    }

    private void OnEnable()
    {
        ConversationManager.OnConversationEnded += MyEndEventMethod;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationEnded -= MyEndEventMethod;
    }

    private void MyEndEventMethod()
    {
        isTalking = false;
        player.GetComponent<PlayerMovement>().enabled = true;
    }

}
