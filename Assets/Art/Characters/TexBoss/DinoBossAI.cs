using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DinoBossAI : DinoAI
{
    private float specialCooldown = 8f;
    private float specialTimer = 0f;
    private bool isUsingSpecial = false;
    public GameObject muzzleFireBreath;
    
    protected override void Start()
    {
        base.Start();
        if(muzzleFireBreath != null)
            muzzleFireBreath.GetComponent<ParticleSystem>().Stop();
    }

    protected override void Update()
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
                specialTimer += Time.deltaTime;
                if (agent.enabled && agent.isOnNavMesh)    
                {            
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                }
                SetMovement(0.5f);

                if (distance <= attackRange && !isUsingSpecial)
                    MeleeAttack();
                else if (distance >= rangedAttackRange && distance <= detectionRange && shootCooldown <= shootTimer && !isUsingSpecial)
                    RangedAttack();
                
                break;
        }
        
        if (this.GetComponent<EnemyHealth>().currentHealth <= 
            this.GetComponent<EnemyHealth>().stats.maxHealth / 2) 
        {
            tripleShot = true;
        }

        if (specialCooldown <= specialTimer)
        {
            float decision = Random.value;
            if (decision < 0.6f && tripleShot)
                BreathFire();
            else
                RainFire();
        }
        
    }

    void RainFire()
    {
        StartCoroutine(RainFireRoutine());
    }

    void BreathFire()
    {
        StartCoroutine(BreathFireRoutine());
    }

    IEnumerator RainFireRoutine()
    {
        isBusy = true;
        isUsingSpecial = true;
        currentState = EnemyState.Shoot;
        if (agent.enabled && agent.isOnNavMesh)
            agent.isStopped = true;

        SetMovement(0f);
        animator.Play("ShootUp");
        yield return new WaitForSeconds(2.5f);

        currentState = EnemyState.Chase;
        if (agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;
        isBusy = false;
        isUsingSpecial = false;
        specialTimer = 0;
    }

    void ShootUpward()
    {
        if (fire != null && projectileSpawnPoint != null)
        {
            AudioManager.audioManager.PlaySFX(0);
            if(fireMuzzle != null)
                fireMuzzle.GetComponent<ParticleSystem>().Play();
            GameObject projectile = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }
    }
    IEnumerator RainDown()
    {
        for (int i = 0; i < 16; i++)
        {
            GameObject projectile;
            Vector3 abovePlayerTransform = new Vector3(player.transform.position.x + Random.Range(-5.0f, 5.0f), 
                player.transform.position.y + 20f, 
                player.transform.position.z + Random.Range(-5.0f, 5.0f));
            projectile = Instantiate(fire, abovePlayerTransform, player.transform.rotation);
            projectile.transform.Rotate(fire.transform.rotation.x + 90, fire.transform.rotation.y, fire.transform.rotation.z);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator BreathFireRoutine()
    {
        isBusy = true;
        isUsingSpecial = true;
        currentState = EnemyState.Shoot;
        if (agent.enabled && agent.isOnNavMesh)
            agent.isStopped = true;

        SetMovement(0f);
        animator.Play("BreathAttack");
        AudioManager.audioManager.PlaySFX(0);

        yield return new WaitForSeconds(0.1f);
        if(muzzleFireBreath != null)
        {
            muzzleFireBreath.GetComponent<ParticleSystem>().Play();
            muzzleFireBreath.GetComponent<BoxCollider>().enabled = true;
        }
        
        yield return new WaitForSeconds(0.9f);

        if (agent.enabled && agent.isOnNavMesh)
            agent.isStopped = false;
        currentState = EnemyState.Chase;
        isBusy = false;
        specialTimer = 0;

        yield return new WaitForSeconds(5.0f);
        if(muzzleFireBreath != null)
        {
            muzzleFireBreath.GetComponent<ParticleSystem>().Stop();
            muzzleFireBreath.GetComponent<BoxCollider>().enabled = false;
        }
        isUsingSpecial = false;
    }

}
