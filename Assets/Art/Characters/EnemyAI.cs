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
        Retreat
    }

    [SerializeField] protected int attackPower = 5;

    [Header("References")]
    [SerializeField] protected Transform player;
    [SerializeField] protected Transform projectileSpawnPoint;
    [SerializeField] protected GameObject fire;

    protected UnityEngine.AI.NavMeshAgent agent;
    protected Animator animator;
    
    [Header("Movement")]
    [SerializeField] protected float detectionRange = 20f;
    [SerializeField] protected float attackRange = 2f;
    [SerializeField] protected float rangedAttackRange = 4f;

    [SerializeField] protected float rotationSpeed = 360f;

    [Header("Timing")]
    [SerializeField] protected float attackDuration = 1.0f;


    protected EnemyState currentState = EnemyState.Idle;
    protected bool isBusy;
    protected void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

    }


    protected void SetMovement(float speed)
    {
        animator.SetFloat("MovementSpeed", speed, 0f, Time.deltaTime);
    }

    private void Attack()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(attackPower);
    }


    void ShootE()
    {
        // Spawn projectile
        if (fire != null && projectileSpawnPoint != null)
        {
            GameObject projectile = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }
    }

}
