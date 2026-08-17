using UnityEngine;
using System.Collections;
using DialogueEditor;

public class NPCInteractble : MonoBehaviour
{
    private GameObject player;
    public GameObject combatManager;

    public NPCConversation conversation;
    private bool isTalking = false;
    private bool wasTalking = false;

    [SerializeField] private float turnSpeed = 180f; // degrees per second
    [SerializeField] private float facingThreshold = 90f; // degrees
    private bool challengeAccepted = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (ConversationManager.Instance.IsConversationActive)
        {
            isTalking = true;
            player.GetComponent<PlayerMovement>().enabled = false;
        }
        else
        {
            isTalking = false;
            if (!challengeAccepted)
                player.GetComponent<PlayerMovement>().enabled = true;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") 
        && other.GetComponent<PlayerMovement>().m_talkAction.WasPressedThisFrame()
        && !isTalking
        && other.GetComponent<PlayerMovement>().IsGrounded())
        {
            other.GetComponent<PlayerMovement>().SetLocomotive(0f);
            StartCoroutine(Turn());

            //if (!IsFacingPlayer() && isTalking)
                //anim.SetFloat("Direction", 1f, 1f, Time.deltaTime);
            //else
                //anim.SetFloat("Direction", 0f, 1f, Time.deltaTime);
        }
            
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

    public void AcceptChallenge()
    {
        challengeAccepted = true;
    }
    public void EndChallenge()
    {
        challengeAccepted = false;
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
        if (challengeAccepted)
            combatManager.GetComponent<CombatManager>().StartCombat();
    }

}
