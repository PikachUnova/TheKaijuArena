using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AI;

public class PlayerMeleeAttack : MonoBehaviour
{
    public CharacterStats stats;
    [SerializeField] private float knockbackForce = 150f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
            
            if (enemy != null)
            {
                enemy.TakeDamage(stats.attackPower);
            }

            // Knockback
            NavMeshAgent agent = other.GetComponentInParent<NavMeshAgent>();
            if (agent != null)
            {
                Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
                knockbackDir.y = 0f; // Keep on NavMesh plane

                StartCoroutine(KnockbackNavMesh(agent, knockbackDir));
            }
        }
    }

    private IEnumerator KnockbackNavMesh(NavMeshAgent agent, Vector3 direction)
    {
        agent.isStopped = false;

        float elapsed = 0f;
        while (elapsed < 1f)
        {
            float strength = Mathf.Lerp(knockbackForce, 0f, elapsed / 1f);
            agent.transform.position += direction * strength * Time.deltaTime;

            elapsed += Time.deltaTime;
            yield return null;
        }

        agent.isStopped = true; 
    }
}