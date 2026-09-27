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

    // Projectile types
    public enum projectileType { fire, homingFire, ice}
    private projectileType currentType = 0;

    void Start()
    {
        fireMuzzle.GetComponent<ParticleSystem>().Stop();
        fireMuzzleHome.GetComponent<ParticleSystem>().Stop();
        iceMuzzle.GetComponent<ParticleSystem>().Stop();
    }

    public void Shoot()
    {
        if (fire == null || fireHome == null || ice == null)
            return;

        if ((int)currentType == 2)
        {
            AudioManager.audioManager.PlaySFX(6);
            Instantiate(ice, this.transform.position, this.transform.rotation);
            iceMuzzle.GetComponent<ParticleSystem>().Play();
        }
        else if ((int)currentType == 1)
        {
            AudioManager.audioManager.PlaySFX(0);
            Instantiate(fireHome, this.transform.position, this.transform.rotation);
            fireMuzzleHome.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            AudioManager.audioManager.PlaySFX(0);
            Instantiate(fire, this.transform.position, this.transform.rotation);
            fireMuzzle.GetComponent<ParticleSystem>().Play();
        }
    }

    public void ShootUpward()
    {
        if (fire == null || fireHome == null || ice == null)
            return;

        GameObject projectile;
        if (SaveManager.Instance.saveData.playerSkills >= 3)
        {
            float randomType = Random.Range(0f,100f);
            if (randomType < 50.0f)
            {
                AudioManager.audioManager.PlaySFX(0);
                projectile = Instantiate(fire, this.transform.position, this.transform.rotation);
            }
            else if (randomType < 75.0f)
            {
                AudioManager.audioManager.PlaySFX(0);
                projectile = Instantiate(fireHome, this.transform.position, this.transform.rotation);
            }
            else
            {
                AudioManager.audioManager.PlaySFX(6);
                projectile = Instantiate(ice, this.transform.position, this.transform.rotation);
            }
        }
        else if (SaveManager.Instance.saveData.playerSkills == 2)
        {
            float randomType = Random.Range(0f,100f);
            AudioManager.audioManager.PlaySFX(0);
            if (randomType < 75.0f)
                projectile = Instantiate(fire, this.transform.position, this.transform.rotation);
            else
                projectile = Instantiate(fireHome, this.transform.position, this.transform.rotation);
        }
        else
        {
            AudioManager.audioManager.PlaySFX(0);
            projectile = Instantiate(fire, this.transform.position, this.transform.rotation);
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


            if (SaveManager.Instance.saveData.playerSkills >= 3)
            {
                float randomType = Random.Range(0f,100f);
                if (randomType < 50.0f)
                {
                    projectile = Instantiate(fire, abovePlayerTransform, objectPos.rotation);
                    projectile.transform.Rotate(fire.transform.rotation.x + 90, fire.transform.rotation.y, fire.transform.rotation.z);
                }
                else if (randomType < 75.0f)
                {
                    projectile = Instantiate(fireHome, abovePlayerTransform, objectPos.rotation);
                    projectile.transform.Rotate(fireHome.transform.rotation.x + 90, fireHome.transform.rotation.y, fireHome.transform.rotation.z);
                }
                else
                {
                    projectile = Instantiate(ice, abovePlayerTransform, objectPos.rotation);
                    projectile.transform.Rotate(ice.transform.rotation.x + 90, ice.transform.rotation.y, ice.transform.rotation.z);
                }
            }
            else if (SaveManager.Instance.saveData.playerSkills == 2)
            {
                float randomType = Random.Range(0f,100f);
                if (randomType < 75.0f)
                {
                    projectile = Instantiate(fire, abovePlayerTransform, objectPos.rotation);
                    projectile.transform.Rotate(fire.transform.rotation.x + 90, fire.transform.rotation.y, fire.transform.rotation.z);
                }
                else
                {
                    projectile = Instantiate(fireHome, abovePlayerTransform, objectPos.rotation);
                    projectile.transform.Rotate(fireHome.transform.rotation.x + 90, fireHome.transform.rotation.y, fireHome.transform.rotation.z);
                }
            }
            else
            {
                projectile = Instantiate(fire, abovePlayerTransform, objectPos.rotation);
                projectile.transform.Rotate(fire.transform.rotation.x + 90, fire.transform.rotation.y, fire.transform.rotation.z);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SwitchProjectile()
    {
        if (SaveManager.Instance.saveData.playerSkills != 0)
            currentType++;

        if (SaveManager.Instance.saveData.playerSkills == 3 && (int)currentType >= 3)
            currentType = 0;
        else if (SaveManager.Instance.saveData.playerSkills == 2 && (int)currentType >= 2)
            currentType = 0;
        Debug.Log(currentType);
    }
    
}