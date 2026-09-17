using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DinoAI : EnemyAI
{
    [Header("Shooting")]
    [SerializeField] protected float shootCooldown = 5f;
    [SerializeField] protected float aimDuration = 0.5f;
    [SerializeField] protected int minNumberOfShots = 1;
    [SerializeField] protected int maxNumberOfShots = 3;
    protected float shootTimer = 0;
    protected bool tripleShot = false;

    protected override void Start()
    {
        base.Start();
    }

    protected virtual void Update()
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

        int numShots = Random.Range(minNumberOfShots, maxNumberOfShots + 1);

        // Aim at the player
        Vector3 targetPosition;
        for (int i = 0; i < numShots; i++)
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

            if (!tripleShot)
            {
                GameObject projectile = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            } 
            else
            {
                GameObject projectile1 = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                GameObject projectile2 = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                GameObject projectile3 = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                projectile1.transform.Rotate(projectileSpawnPoint.transform.rotation.x, projectileSpawnPoint.transform.rotation.y + 10, projectileSpawnPoint.transform.rotation.z);
                projectile3.transform.Rotate(projectileSpawnPoint.transform.rotation.x, projectileSpawnPoint.transform.rotation.y - 10, projectileSpawnPoint.transform.rotation.z);
            }
        }
        
    }

}
