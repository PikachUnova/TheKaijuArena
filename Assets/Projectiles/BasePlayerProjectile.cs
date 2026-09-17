using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 16f;
    [SerializeField] protected int attackPower = 8;

    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected GameObject impact;

    // Projectile types
    public enum projectileType {fire, ice}

    protected virtual void Start()
    {
        Destroy(this.gameObject, 5.0f); // Exist until 5 seconds passed
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) // Damage Enemy
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy.currentHealth <= 0) return; // Do nothing
            
            enemy.TakeDamage(attackPower);

            if (impact != null)
                Instantiate(impact, transform.position, transform.rotation);
            AudioManager.audioManager.PlaySFX(2);
            Destroy(this.gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {   
            if (impact != null)
                Instantiate(impact, transform.position, transform.rotation);
            AudioManager.audioManager.PlaySFX(2);
            Destroy(this.gameObject);
        }
    }
}