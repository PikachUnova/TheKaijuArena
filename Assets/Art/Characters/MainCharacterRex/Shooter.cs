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

    public GameObject ice;

    // Muzzles
    public ParticleSystem fireMuzzle;
    public ParticleSystem fireMuzzleHome;
    public ParticleSystem iceMuzzle;

    void Start()
    {
        fireMuzzle.GetComponent<ParticleSystem>().Stop();
        fireMuzzleHome.GetComponent<ParticleSystem>().Stop();
        iceMuzzle.GetComponent<ParticleSystem>().Stop();
    }

    public void Shoot()
    {
        ShootFire();
    }

    void ShootFire()
    {
        int randomFire = Random.Range(1, 3); 
        if (randomFire == 0)
        {
            AudioManager.audioManager.PlaySFX(0);
            Instantiate(fireHome, this.transform.position, this.transform.rotation);
            fireMuzzleHome.GetComponent<ParticleSystem>().Play();
        }
        else if (randomFire == 1)
        {
            AudioManager.audioManager.PlaySFX(0);
            Instantiate(fire, this.transform.position, this.transform.rotation);
            fireMuzzle.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            AudioManager.audioManager.PlaySFX(6);
            Instantiate(ice, this.transform.position, this.transform.rotation);
            iceMuzzle.GetComponent<ParticleSystem>().Play();
        }
    }
    
}