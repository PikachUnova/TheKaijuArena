using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AnklyoAI : EnemyAI
{

    private bool isRolling;
    [SerializeField] private float rollRange;
    [SerializeField] private float rollSpeed;
    [SerializeField] private float rollDuration = 2f;
    [SerializeField] private float rollCooldown = 5f;
    private float rollTimer;

    // Idles at first and then start the combat
    void Start()
    {
        base.Start();
        StartCoroutine(Idle());
    }

    void Update()
    {
        if (player == null || isBusy
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
                SetMovement(0f);
                break;

            case EnemyState.Chase:
                rollTimer += Time.deltaTime;
                agent.isStopped = false;
                agent.SetDestination(player.position);
                SetMovement(1f);

                if (distance <= attackRange)
                {
                    StartCoroutine(AttackRoutine());
                }
                else if (distance <= rollRange && rollCooldown <= rollTimer)
                {
                    StartCoroutine(RollAttackRoutine());
                }
                break;

            case EnemyState.Retreat:
                StartCoroutine(RetreatRoutine());
                break;
        }
    }

    IEnumerator Idle()
    {
        currentState = EnemyState.Idle;
        SetMovement(0f);
        yield return new WaitForSeconds(2f);
        SetMovement(1f);
        currentState = EnemyState.Chase;
    }

    private IEnumerator RollAttackRoutine()
    {
        isBusy = true;
        currentState = EnemyState.Attack;

        agent.isStopped = true;
        SetMovement(0f);

        // Face the player
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
        
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null) // Invulnerable
            health.SetInvulnerable(true);

        animator.Play("Roll");

        yield return new WaitForSeconds(1f);

        isRolling = true;

        float elapsedTime = 0f;
        while (elapsedTime < rollDuration)
        {
            transform.position += transform.forward * rollSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isRolling = false;
        animator.SetTrigger("Cancel");

        // Become vulnerable again
        if (health != null)
            health.SetInvulnerable(false);

        currentState = EnemyState.Chase;

        agent.isStopped = false;
        isBusy = false;
        rollTimer = 0;
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 2f)
                break;
            yield return null;
        }

        animator.Play("Attack");

        yield return new WaitForSeconds(attackDuration);

        // Decide what the enemy does next.
        float decision = Random.value;
        if (decision <= 0.5f)
        {
            currentState = EnemyState.Retreat;
            agent.isStopped = false;
            isBusy = false;
        }
        else
        {
            agent.isStopped = true;
            SetMovement(0f);
        }
        isBusy = false;
    }

    private IEnumerator RetreatRoutine()
    {
        isBusy = true;
        agent.isStopped = false;
        agent.speed = movementSpeed;
        SetMovement(0.5f);

        while (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance >= rollRange / 2)
                break;

            Vector3 direction = transform.position - player.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Vector3 retreatPosition = transform.position + direction.normalized * 2f;
                agent.SetDestination(retreatPosition);
            }
            yield return null;
        }

        agent.isStopped = false;
        currentState = EnemyState.Chase;
        isBusy = false;
    }

}

