using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireBreath : MonoBehaviour
{
    [SerializeField] private int damageAmount = 4;
    [SerializeField] private float damageCooldown = 0.25f; // Time between 
    public GameObject player;

    private float nextDamageTime = 0f;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && Time.time >= nextDamageTime)
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damageAmount);
            nextDamageTime = Time.time + damageCooldown; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            nextDamageTime = 0f;
    }

}
