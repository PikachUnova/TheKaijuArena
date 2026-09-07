using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DinoAI : EnemyAI
{
    [Header("Shooting")]
    [SerializeField] private float shootCooldown = 5f;
    [SerializeField] private float aimDuration = 3f;
    [SerializeField] private int numberOfShots = 3;
    private float shootTimer = 0;

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (isBusy) return;
        StopWhenPlayerBeaten();

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
                
                    MeleeAttack();
                
                else if (distance >= rangedAttackRange && distance <= detectionRange && shootCooldown <= shootTimer)
                
                    RangedAttack();
                
                break;
        }
    }

    void MeleeAttack()
    {
        StartCoroutine(AttackRoutine());
    }

    protected IEnumerator AttackRoutine()
    {
        isBusy = true;
        currentState = EnemyState.Attack;

        agent.isStopped = true;
        SetMovement(0f);

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
            while (true)
            {
                Vector3 direction = player.position - transform.position;
                direction.y = 0f;

                if (direction.sqrMagnitude < 0.01f) break;

                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                if (Quaternion.Angle(transform.rotation, targetRotation) < 2f) break;

                yield return null;
            }
            
        }
        isBusy = false;
    }

      void RangedAttack()
    {
        StartCoroutine(RangedAttackRoutine());
    }

    protected IEnumerator RangedAttackRoutine()
    {
        isBusy = true;
        currentState = EnemyState.Shoot;

        agent.isStopped = true;
        SetMovement(0f);

        // Aim at the player
        Vector3 targetPosition;
        for (int i = 0; i < numberOfShots; i++)
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
            AudioManager.audioManager.PlaySFX(0);
            GameObject projectile = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }
    }

}
