using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireBreath : MonoBehaviour
{
    [SerializeField] private int damageAmount = 4;
    [SerializeField] private float damageCooldown = 0.25f; // Time between 

    [SerializeField] private bool isEnemyAttack = true; // Time between 

    private float nextDamageTime = 0f;


    private void OnTriggerStay(Collider other)
    {
        if (isEnemyAttack)
        {
            if (other.gameObject.CompareTag("Player") && Time.time >= nextDamageTime)
            {
                other.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageCooldown; 
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Enemy") && Time.time >= nextDamageTime)
            {
                other.GetComponent<EnemyHealth>().TakeDamage(damageAmount);
                nextDamageTime = Time.time + damageCooldown; 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isEnemyAttack)
        {
            if (other.gameObject.CompareTag("Player"))
            nextDamageTime = 0f;
        }
        else
        {
            if (other.gameObject.CompareTag("Enemy"))
            nextDamageTime = 0f;
        }
    }

}
