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
        Retreat
    }

    [Header("References")]
    [SerializeField] private Transform player;

    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    
    [Header("Movement")]
    [SerializeField] private float detectionRange = 12f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float retreatDistance = 3f;

    [Header("Timing")]
    [SerializeField] private float attackDuration = 1.0f;
    [SerializeField] private float minAttackCooldown = 2f;
    [SerializeField] private float maxAttackCooldown = 4f;
    [SerializeField] private float retreatTime = 1f;


    private EnemyState currentState = EnemyState.Idle;
    private bool busy;
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        //armature.transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null || busy)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Idle:

                SetMovement(0f);

                if (distance <= detectionRange)
                    currentState = EnemyState.Chase;

                break;

            case EnemyState.Chase:

                agent.isStopped = false;
                agent.SetDestination(player.position);
                SetMovement(0.5f);

                if (distance <= attackRange)
                    StartCoroutine(AttackRoutine());

                break;
        }
    }

    IEnumerator AttackRoutine()
    {
        busy = true;
        currentState = EnemyState.Attack;

        agent.isStopped = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        SetMovement(0f);

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDuration);

        currentState = EnemyState.Retreat;

        Vector3 retreatDirection =
            (transform.position - player.position).normalized;

        agent.isStopped = false;
        agent.SetDestination(transform.position + retreatDirection * retreatDistance);

        SetMovement(0.5f);

        yield return new WaitForSeconds(retreatTime);

        agent.isStopped = true;
        SetMovement(0f);

        yield return new WaitForSeconds(
            Random.Range(minAttackCooldown, maxAttackCooldown));

        currentState = EnemyState.Chase;
        busy = false;
    }

    private void SetMovement(float speed)
    {
        animator.SetFloat("MovementSpeed", speed, 0.2f, Time.deltaTime);
    }

}
