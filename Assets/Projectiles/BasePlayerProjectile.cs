using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerProjectile : MonoBehaviour
{
    [SerializeField] protected ParticleSystem particle;
    [SerializeField] protected float speed = 20;
    [SerializeField] protected int attackPower = 10;

    protected AudioSource audioSource;
    [SerializeField] protected AudioClip hitObstacle, hitEnemy;

    [SerializeField] protected LayerMask groundMask;
    [SerializeField] protected LayerMask obstacleLayer; // The layer for obstacles that block line of sight
    [SerializeField] protected GameObject residue;

    //Fire Effect Projectiles
    [SerializeField] protected int burnRate = 25;




    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(this.gameObject, 5.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move projectile
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}