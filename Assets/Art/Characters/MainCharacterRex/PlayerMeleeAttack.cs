using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    public CharacterStats stats;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponentInParent<EnemyHealth>();
            
            if (enemy != null)
            {
                enemy.TakeDamage(stats.attackPower);
                Debug.Log("Hit");
            }
        }
    }
}