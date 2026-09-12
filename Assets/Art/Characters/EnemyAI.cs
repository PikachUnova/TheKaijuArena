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
        Defeated,
        Wait
    }

    [SerializeField] protected int attackPower = 5;

    [Header("References")]
    [SerializeField] protected Transform player;
    [SerializeField] protected Transform projectileSpawnPoint;
    [SerializeField] protected GameObject fire;
    [SerializeField] protected Collider [] attackTriggers;

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

    [Header("Freeze Effect")]
    [SerializeField] private Renderer enemyRenderer;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material frozenMaterial;
    public GameObject freezeEffect; // Visual effect for freezing
    private bool isFrozen = false;

    [Header("States")]
    protected EnemyState currentState = EnemyState.Idle;
    protected bool isBusy;
    [SerializeField] protected bool fallOnDefeat = false;

    protected virtual void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.speed = movementSpeed;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected void StopWhenPlayerBeaten()
    {
        if (player == null || player.gameObject.GetComponent<PlayerHealth>().currentHealth <= 0)
        {
            currentState = EnemyState.Idle;
            agent.speed = 0;
            agent.angularSpeed = 0;
        }
    }

    public void OnDeath()
    {
        if (this.gameObject.GetComponent<EnemyHealth>().currentHealth <= 0)
        {
            currentState = EnemyState.Defeated;
            isBusy = false;

            StopAllCoroutines();
            DisableAttackCollider(1);
            if (agent != null)
            {
                agent.speed = 0;
                agent.angularSpeed = 0;
                agent.enabled = false;
            }

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null && fallOnDefeat)
            {
                rb.isKinematic = false; // Turn off Kinematic so forces apply
                rb.useGravity = true;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentState == EnemyState.Defeated && collision.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
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

    public void EnableAttackCollider(int move)
    {
        AudioManager.audioManager.PlaySFX(3);
        //Debug.Log(move-1);
        attackTriggers[move-1].GetComponent<Collider>().enabled = true;
    }

    public void DisableAttackCollider(int move)
    {
        attackTriggers[move-1].GetComponent<Collider>().enabled = false;
    }

    public void Freeze(float time)
    {
        // No change if already frozen
        if (isFrozen) return;

        if (freezeEffect != null)
            Instantiate(freezeEffect, this.transform.position, this.transform.rotation);

        if (enemyRenderer != null && frozenMaterial != null)
        {
            Material[] currentMats = enemyRenderer.materials;
            Material[] newMats = new Material[currentMats.Length + 1];

            for (int i = 0; i < currentMats.Length; i++)
                newMats[i] = currentMats[i];
            
            newMats[newMats.Length - 1] = frozenMaterial;
            enemyRenderer.materials = newMats;
        }
        
        this.enabled = false;
        agent.velocity = Vector3.zero;
        agent.speed = 0;
        agent.isStopped = true;
        animator.enabled = false;
        isFrozen = true;
        StopAllCoroutines();
        StartCoroutine(UnfreezeEnemyCoroutine(time));
        
    }
    private IEnumerator UnfreezeEnemyCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        if (isFrozen)
        {
            Unfreeze();
        }
    }

    public void Unfreeze()
    {
        isFrozen = false;
        this.enabled = true;
        agent.speed = movementSpeed;
        agent.isStopped = false;
        animator.enabled = true;
        currentState = EnemyState.Chase;
        if (enemyRenderer != null && frozenMaterial != null)
        {
            Material[] currentMats = enemyRenderer.materials;
            Material[] newMats = new Material[currentMats.Length - 1];

            newMats[0] = currentMats[0];
            newMats[0] = originalMaterial;
            enemyRenderer.materials = newMats;
        }
        isBusy = false;
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }

}
