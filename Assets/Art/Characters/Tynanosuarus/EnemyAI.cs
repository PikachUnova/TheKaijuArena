using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
