using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PteraAI : EnemyAI
{
    [Header("Flight")]
    [SerializeField] private float takeOffDelay = 2f;
    [SerializeField] private float flightHeight = 3f;
    [SerializeField] private float flightSpeed = 5f;

    [Header("Shooting")]
    [SerializeField] private float shootCooldown = 2f;
    [SerializeField] private float aimDuration = 3f;
    private float nextShootTime;


    private bool isFlying = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        StartCoroutine(Idle());
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || isBusy || !isFlying 
        || this.gameObject.GetComponent<EnemyHealth>().currentHealth <= 0
        || player.gameObject.GetComponent<PlayerHealth>().currentHealth <= 0)
            return;
        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Idle:

                if (distance <= detectionRange)
                    currentState = EnemyState.Chase;

                break;

            case EnemyState.Chase:

                agent.isStopped = false;
                agent.speed = flightSpeed;

                agent.SetDestination(player.position);

                if (distance <= attackRange)
                {
                    StartCoroutine(AttackRoutine());
                }
                else if (distance <= rangedAttackRange)
                {
                    StartCoroutine(RangedAttackRoutine());
                }

                break;
        }
        
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(2f);
        isFlying = true;
        agent.baseOffset = flightHeight; 
        SetMovement(1f);
    }

    private IEnumerator AttackRoutine()
    {
        isBusy = true;
        currentState = EnemyState.Attack;

        agent.baseOffset = flightHeight / 2; 
        agent.isStopped = true;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDuration);
        player.GetComponent<PlayerHealth>().TakeDamage(attackPower);

        // Decide what the enemy does next.
        float decision = Random.value;
        if (decision <= 0.5f)
        {
            agent.isStopped = true;
            currentState = EnemyState.Chase;
        }
        else
        {
            agent.isStopped = true;
            currentState = EnemyState.Chase;
        }
        agent.baseOffset = flightHeight; 
        isBusy = false;
    }

    protected IEnumerator RangedAttackRoutine()
    {
        isBusy = true;
        currentState = EnemyState.Shoot;

        agent.isStopped = true;

        // Face the player
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);

        transform.LookAt(targetPosition);

        // Spawn projectile
        if (fire != null && projectileSpawnPoint != null)
        {
            animator.SetTrigger("ShootEvent");
            GameObject projectile = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }

        nextShootTime = Time.time + shootCooldown;

        yield return new WaitForSeconds(aimDuration);

        // Chase again
        currentState = EnemyState.Chase;
        agent.isStopped = false;
        isBusy = false;
    }


}
