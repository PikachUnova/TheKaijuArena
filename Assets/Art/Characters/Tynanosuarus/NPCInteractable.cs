using UnityEngine;
using System.Collections;
using DialogueEditor;

public class NPCInteractable : MonoBehaviour
{
    private GameObject player;
    public GameObject combatManager;

    public NPCConversation conversation;
    public NPCConversation conversationWin;
    public NPCConversation conversationLoss;

    private bool isTalking = false;
    [SerializeField] private float turnSpeed = 180f; // degrees per second
    [SerializeField] private float facingThreshold = 90f; // degrees

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
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

    public void StartCoversationWin()
    {
        ConversationManager.Instance.StartConversation(conversationWin);
    }
    public void StartCoversationLoss()
    {
        ConversationManager.Instance.StartConversation(conversationLoss);
    }

    private void OnEnable()
    {
        // Subscribe to the end conversation event
        ConversationManager.OnConversationEnded += MyEndEventMethod;
    }

    private void OnDisable()
    {
        // Always unsubscribe when the object is disabled/destroyed
        ConversationManager.OnConversationEnded -= MyEndEventMethod;
    }

    private void MyEndEventMethod()
    {
        if (!CombatLevelSelector.levelSelector.gameObject.activeSelf)
        {
            isTalking = false;
            player.GetComponent<PlayerMovement>().enabled = true;
        }/*
        else
        {
            CombatLevelSelector.levelSelector.gameObject.SetActive(true);
            Debug.Log("Your conversation has ended!");
        }*/
    }

}
