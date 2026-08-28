using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections;

public class DinoAI : EnemyAI
{
    [Header("Shooting")]
    [SerializeField] private float shootCooldown = 5f;
    [SerializeField] private float aimDuration = 3f;
    private float shootTimer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

        if (isBusy)
            return;

        if  (player == null || player.gameObject.GetComponent<PlayerHealth>().currentHealth <= 0)
            currentState = EnemyState.Idle;
        
        
        if (this.gameObject.GetComponent<EnemyHealth>().currentHealth <= 0)
        {
            isBusy = true;
            StopAllCoroutines();
            currentState = EnemyState.Defeated;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            enabled = false;
        }



        float distance = Vector3.Distance(transform.position, player.position);
        switch (currentState)
        {
            case EnemyState.Idle:

                SetMovement(0f);

                if (distance <= detectionRange)
                    currentState = EnemyState.Chase;
                break;

            case EnemyState.Chase:

                shootTimer += Time.deltaTime;
                agent.isStopped = false;
                agent.SetDestination(player.position);
                SetMovement(0.5f);

                if (distance <= attackRange)
                    StartCoroutine(AttackRoutine());
                else if (distance >= rangedAttackRange && distance <= detectionRange && shootCooldown <= shootTimer)
                    StartCoroutine(RangedAttackRoutine());
                break;
        }
    }

    protected IEnumerator AttackRoutine()
    {
        isBusy = true;
        currentState = EnemyState.Attack;

        agent.isStopped = true;
        SetMovement(0f);
        
        while (true)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.01f)
                break;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 2f)
                break;

            yield return null;
        }

        animator.Play("Attack");
        yield return new WaitForSeconds(attackDuration);

        // Decide what the enemy does next.
        float decision = Random.value;
        if (decision <= 0.8f)
        {
            yield return StartCoroutine(BackJumpRoutine()); // Dodge backwards
        }
        else
        {
            agent.isStopped = true;
            SetMovement(0f);
            currentState = EnemyState.Chase;
        }
        isBusy = false;
    }

    protected IEnumerator RangedAttackRoutine()
    {
        isBusy = true;
        currentState = EnemyState.Shoot;

        agent.isStopped = true;
        SetMovement(0f);

        // Aim at the player
        Vector3 targetPosition;
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(aimDuration);
            targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(targetPosition);
            animator.Play("Shoot");
        }

        // Chase again
        currentState = EnemyState.Chase;
        agent.isStopped = false;
        isBusy = false;
        shootTimer = 0;
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
