using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeleeAttackTrigger : MonoBehaviour
{
    public CharacterStats stats;
    [SerializeField] private float knockbackForce = 20f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(stats.attackPower);
        }

        // Knockback
        CharacterController player = other.GetComponent<CharacterController>();

        if (player != null)
        {
            Vector3 knockbackDirection =
                (other.transform.position - transform.position).normalized;

            knockbackDirection.y = 0.2f;

            StartCoroutine(Knockback(player, knockbackDirection));
        }
        
    }

    private IEnumerator Knockback(
    CharacterController player,
    Vector3 direction)
{
    float knockbackDuration = 0.5f;

    float elapsed = 0f;

    while (elapsed < knockbackDuration)
    {
        float strength = Mathf.Lerp(knockbackForce, 0f, elapsed / knockbackDuration);

        player.Move(direction * strength * Time.deltaTime);

        elapsed += Time.deltaTime;

        yield return null;
    }
}

}
