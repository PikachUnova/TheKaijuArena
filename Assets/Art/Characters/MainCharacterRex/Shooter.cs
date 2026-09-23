using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    // Projectiles
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

    public void ShootUpward()
    {
        if (fire != null)
        {
            AudioManager.audioManager.PlaySFX(0);
            GameObject projectile = Instantiate(fire, this.transform.position, this.transform.rotation);
        }
    }
    public IEnumerator RainDown(Transform objectPos)
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject projectile;
            Vector3 abovePlayerTransform = new Vector3(objectPos.position.x + Random.Range(-10.0f, 10.0f), 
                objectPos.position.y + 20f, 
                objectPos.position.z + Random.Range(-10.0f, 10.0f));
            projectile = Instantiate(fire, abovePlayerTransform, objectPos.rotation);
            projectile.transform.Rotate(fire.transform.rotation.x + 90, fire.transform.rotation.y, fire.transform.rotation.z);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void ShootFire()
    {
        int randomFire = Random.Range(1, 2); 
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