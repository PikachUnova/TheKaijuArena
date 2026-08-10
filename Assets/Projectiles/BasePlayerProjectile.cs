using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerProjectile : MonoBehaviour
{
    [SerializeField] protected float speed = 20;
    [SerializeField] protected int attackPower = 8;


    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected GameObject impact;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 5.0f); // Exist until 5 seconds passed
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move projectile
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) // Damage Enemy
        {
            other.GetComponent<EnemyHealth>().TakeDamage(attackPower);
            Instantiate(impact, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Grass"))
        {
            Instantiate(impact, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}