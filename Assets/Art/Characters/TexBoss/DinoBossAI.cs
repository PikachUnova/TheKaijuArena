using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class DinoBossAI : DinoAI
{
    private float specialCooldown = 8f;
    private float specialTimer = 0f;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update(); 
        if (this.GetComponent<EnemyHealth>().currentHealth <= 
            this.GetComponent<EnemyHealth>().stats.maxHealth / 2) 
        {
            tripleShot = true;
        }

        if (currentState == EnemyState.Chase)
            specialTimer += Time.deltaTime;
        

        if (specialCooldown <= specialTimer)
        {
            float decision = Random.value;
            if (decision <= 0.5f)
                RainFire();
            else
                BreathFire();
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
        currentState = EnemyState.Shoot;
        if (agent.enabled && agent.isOnNavMesh)
            agent.isStopped = true;
        SetMovement(0f);
        animator.Play("ShootUp");
        yield return new WaitForSeconds(2.5f);

        // Chase again
        currentState = EnemyState.Chase;
        agent.isStopped = false;
        isBusy = false;
        specialTimer = 0;
    }

    void ShootUpward()
    {
        if (fire != null && projectileSpawnPoint != null)
        {
            GameObject projectile = Instantiate(fire, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }
    }
    IEnumerator RainDown()
    {
        for (int i = 0; i < 16; i++)
        {

            GameObject projectile;
            Vector3 abovePlayerTransform = new Vector3(player.transform.position.x + Random.Range(0.0f, 20.0f), 
                player.transform.position.y + 15, 
                player.transform.position.z + Random.Range(0.0f, 20.0f));
            projectile = Instantiate(fire, abovePlayerTransform, player.transform.rotation);
            projectile.transform.Rotate(fire.transform.rotation.x + 90, fire.transform.rotation.y, fire.transform.rotation.z);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator BreathFireRoutine()
    {
        isBusy = true;
        currentState = EnemyState.Shoot;
        if (agent.enabled && agent.isOnNavMesh)
            agent.isStopped = true;
        SetMovement(0f);
        yield return new WaitForSeconds(0.2f);
        animator.Play("Breath");
        currentState = EnemyState.Chase;
        yield return new WaitForSeconds(5f);

        // Chase again
        agent.isStopped = false;
        isBusy = false;
        specialTimer = 0;
    }

}
