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
        currentState = EnemyState.Dash;

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
        EnableAttackCollider();

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
        DisableAttackCollider();

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
        
        agent.isStopped = true;
        SetMovement(0f);
        currentState = EnemyState.Chase;
        
        isBusy = false;
    }



}

