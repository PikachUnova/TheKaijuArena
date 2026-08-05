using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject fire;
    public GameObject fireHome;


    // Prevents overshooting
    public bool canShoot;
    public float timeBetweenShots = 0.5f;
    private float timeUntilNextShot;

    // Projectile types
    public enum projectileType {fire}

    // Muzzles
    public ParticleSystem fireMuzzle;
    public ParticleSystem fireMuzzleHome;

    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        fireMuzzle.GetComponent<ParticleSystem>().Stop();
        fireMuzzleHome.GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeUntilNextShot) // The player can shoot once again
            canShoot = true; 
    }

    public void Shoot()
    {
        canShoot = false;
        timeUntilNextShot = Time.time + timeBetweenShots;
        ShootFire();
    }

    void ShootFire()
    {
        int randomFire = Random.Range(0, 3); 
        if (randomFire == 0)
        {
            Instantiate(fireHome, this.transform.position, this.transform.rotation);
            fireMuzzleHome.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            Instantiate(fire, this.transform.position, this.transform.rotation);
            fireMuzzle.GetComponent<ParticleSystem>().Play();
        }
    }
    
}