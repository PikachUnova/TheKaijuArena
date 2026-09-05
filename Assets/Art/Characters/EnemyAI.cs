using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Chase,
        Attack,
        Shoot,
        Retreat,
        Dash,
        Defeated
    }

    [SerializeField] protected int attackPower = 5;

    [Header("References")]
    [SerializeField] protected Transform player;
    [SerializeField] protected Transform projectileSpawnPoint;
    [SerializeField] protected GameObject fire;
    [SerializeField] protected Collider attackTrigger;

    protected NavMeshAgent agent;
    protected Animator animator;
    
    [Header("Movement")]
    [SerializeField] protected float detectionRange = 20f;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float rangedAttackRange = 4f;

    [SerializeField] protected float movementSpeed = 4f;
    [SerializeField] protected float rotationSpeed = 360f;

    [Header("Timing")]
    [SerializeField] protected float attackDuration = 1.0f;

        [Header("Back Jump")]
        [SerializeField] private float backJumpDistance = 6f;
        [SerializeField] private float backJumpHeight = 1.5f;
        [SerializeField] private float backJumpDuration = 0.5f;


    protected EnemyState currentState = EnemyState.Idle;
    protected bool isBusy;
    protected void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = movementSpeed;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected void SetMovement(float speed)
    {
        animator.SetFloat("MovementSpeed", speed, 0f, Time.deltaTime);
    }

    protected IEnumerator BackJumpRoutine()
    {
        if (this.gameObject.GetComponent<EnemyHealth>().currentHealth <= 0)
                yield break;
        currentState = EnemyState.Retreat;

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        SetMovement(0f);

        // Keep facing the player
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(directionToPlayer);
        }

        // The direction opposite from the player
        Vector3 backwardDirection = -transform.forward;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + backwardDirection * backJumpDistance;

        float elapsedTime = 0f;

        animator.Play("Jump");

        while (elapsedTime < backJumpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / backJumpDuration;

            // Smooth horizontal movement
            Vector3 position = Vector3.Lerp(startPosition, endPosition, t);

            // Parabolic jump
            position.y += Mathf.Sin(t * Mathf.PI) * backJumpHeight;
            transform.position = position;

            yield return null;
        }

        transform.position = endPosition;

        agent.Warp(transform.position);
        currentState = EnemyState.Chase;
        agent.isStopped = false;
    }

    public int GetAttackPower()
    {
        return attackPower;
    }

    public void EnableAttackCollider()
    {
        AudioManager.audioManager.PlaySFX(3);
        attackTrigger.GetComponent<Collider>().enabled = true;
    }

    public void DisableAttackCollider()
    {
        attackTrigger.GetComponent<Collider>().enabled = false;
    }





}
