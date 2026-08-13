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
    [SerializeField] private float shootCooldown = 3f;
    private float shootTimer;

    [Header("Dash Attack")]
    [SerializeField] private float dashRange = 15f;
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashHeight = 1.5f;
    [SerializeField] private float dashCooldown = 5f;
    private float dashTimer;
    private bool isDashing;

    private bool isFlying = false;

    void Start()
    {
        base.Start();
        StartCoroutine(Idle());
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || isBusy || !isFlying 
            || player.gameObject.GetComponent<PlayerHealth>().currentHealth <= 0)
            return;
        
        if (this.gameObject.GetComponent<EnemyHealth>().currentHealth <= 0)
        {
            this.enabled = false;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }
        

        float distance = Vector3.Distance(transform.position, player.position);
        switch (currentState)
        {
            case EnemyState.Idle:

                if (distance <= detectionRange)
                    currentState = EnemyState.Chase;
                break;

            case EnemyState.Chase:

                shootTimer += Time.deltaTime;
                dashTimer += Time.deltaTime;
                agent.isStopped = false;
                agent.speed = flightSpeed;
                agent.SetDestination(player.position);

                if (distance <= attackRange)
                    StartCoroutine(AttackRoutine());
                if (distance <= dashRange && dashCooldown <= dashTimer)
                    StartCoroutine(DashAttackRoutine());
                else if (distance <= rangedAttackRange && shootCooldown <= shootTimer)
                    StartCoroutine(RangedAttackRoutine());
                break;
        }
        
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(2f);
        isFlying = true;
        SetFlightHeight(flightHeight, 1f);
        SetMovement(1f);
    }

    private IEnumerator AttackRoutine()
    {
        isBusy = true;
        currentState = EnemyState.Attack;
        yield return new WaitForSeconds(0.5f);
        SetFlightHeight(flightHeight / 2, 0.5f);

        yield return new WaitForSeconds(0.1f);
        agent.isStopped = true;
        yield return new WaitForSeconds(0.4f);
        animator.SetTrigger("Attack");
    
        yield return new WaitForSeconds(attackDuration);

        agent.isStopped = true;
        currentState = EnemyState.Chase;
        SetFlightHeight(flightHeight, 0.5f);
        
        isBusy = false;
    }

    private IEnumerator RangedAttackRoutine()
    {
        isBusy = true;
        currentState = EnemyState.Shoot;

        agent.isStopped = true;

        // Face the player
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPosition);
        animator.Play("Shoot");

        yield return new WaitForSeconds(shootCooldown);

        // Chase again
        currentState = EnemyState.Chase;
        agent.isStopped = false;
        isBusy = false;
        shootTimer = 0;
    }

    private IEnumerator DashAttackRoutine()
    {
        isBusy = true;
        currentState = EnemyState.Dash;

        agent.isStopped = true;
        SetFlightHeight(dashHeight, 0.2f);

        yield return new WaitForSeconds(0.2f);

        // Face the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(direction);

        animator.Play("Dash");

        yield return new WaitForSeconds(0.1f);

        isDashing = true;

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.position += transform.forward * dashSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        isDashing = false;

        // Return to flying height
        SetFlightHeight(flightHeight, 0.5f);
        currentState = EnemyState.Chase;
        agent.isStopped = false;
        isBusy = false;
        dashTimer = 0;
    }

    private void SetFlightHeight(float targetHeight, float duration)
    {
        StartCoroutine(TransitionBaseOffset(targetHeight, duration));
    }

    private IEnumerator TransitionBaseOffset(float targetOffset, float duration)
    {
        float startOffset = agent.baseOffset;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            agent.baseOffset = Mathf.Lerp(startOffset, targetOffset, elapsedTime / duration);
            yield return null;
        }

        agent.baseOffset = targetOffset; // Ensure precise final value
    }

    void ShootE()
    {
        // Spawn projectile
        if (fire != null && projectileSpawnPoint != null)
        {
            GameObject projectile1 = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            GameObject projectile2 = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            GameObject projectile3 = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            projectile1.transform.Rotate(projectileSpawnPoint.transform.rotation.x, projectileSpawnPoint.transform.rotation.y + 10, projectileSpawnPoint.transform.rotation.z);
            projectile3.transform.Rotate(projectileSpawnPoint.transform.rotation.x, projectileSpawnPoint.transform.rotation.y - 10, projectileSpawnPoint.transform.rotation.z);
        }
    }


}
