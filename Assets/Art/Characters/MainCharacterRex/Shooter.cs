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

    public ParticleSystem fireBreath;

    // Projectile types
    public enum projectileType { fire, homingFire, ice}
    private projectileType currentType = 0;

    private bool isIgnited = false;


    void Start()
    {
        fireMuzzle.GetComponent<ParticleSystem>().Stop();
        fireMuzzleHome.GetComponent<ParticleSystem>().Stop();
        iceMuzzle.GetComponent<ParticleSystem>().Stop();
        fireBreath.GetComponent<ParticleSystem>().Stop();
        UIHandler.handler.currentIcon.texture = UIHandler.handler.textureIcons[0];
    }

    public bool CheckEnergyPoints()
    {
        if (UIHandler.handler.GetEnergyPoints() > 0)
            return true;
        return false;
    }

    public void ReplenishEnergyPoints()
    {
        UIHandler.handler.SetEnergyPoints(10);
    }

    

    public void Shoot()
    {
        if (fire == null || fireHome == null || ice == null)
            return;

        if ((int)currentType == 2)
        {
            AudioManager.audioManager.PlaySFX(6);
            if (!isIgnited) {
                Instantiate(ice, this.transform.position, this.transform.rotation);
                UIHandler.handler.UseEnergyPoints();
            }
            else
                MultiShot(ice);
            iceMuzzle.GetComponent<ParticleSystem>().Play();
        }
        else if ((int)currentType == 1)
        {
            AudioManager.audioManager.PlaySFX(0);
            if (!isIgnited) {
                Instantiate(fireHome, this.transform.position, this.transform.rotation);
                UIHandler.handler.UseEnergyPoints();
            }
            else
                MultiShot(fireHome);
            fireMuzzleHome.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            AudioManager.audioManager.PlaySFX(0);
            if (!isIgnited) {
                Instantiate(fire, this.transform.position, this.transform.rotation);
                UIHandler.handler.UseEnergyPoints();
            }
            else
                MultiShot(fire);
            fireMuzzle.GetComponent<ParticleSystem>().Play();
        }
    }

    public void BreathStream()
    {
        fireBreath.GetComponent<ParticleSystem>().Play();
        fireBreath.GetComponent<BoxCollider>().enabled = true;
        StartCoroutine(PowerUpTime(10f));
    }

    public void SetIgnited()
    {
        isIgnited = true;
        StartCoroutine(PowerUpTime(10f));
    }

    private IEnumerator PowerUpTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isIgnited = false;
        fireBreath.GetComponent<BoxCollider>().enabled = false;
        fireBreath.GetComponent<ParticleSystem>().Stop();
    }

    private void MultiShot(GameObject projectile)
    {
        int random = Random.Range(0,10);

        if (random < 5) // triple shot
        {
            GameObject shot1 = projectile;
            GameObject shot3 = projectile;
            shot1 = Instantiate(shot1, this.transform.position, this.transform.rotation);
            Instantiate(projectile, this.transform.position, this.transform.rotation);
            shot3 = Instantiate(shot3, this.transform.position, this.transform.rotation);
            shot1.transform.Rotate(projectile.transform.rotation.x, projectile.transform.rotation.y + 10, projectile.transform.rotation.z);
            shot3.transform.Rotate(projectile.transform.rotation.x, projectile.transform.rotation.y - 10, projectile.transform.rotation.z);
        }
        else if (random >= 5 && random < 8)
        {
            GameObject shot1 = projectile;
            GameObject shot2 = projectile;
            GameObject shot3 = projectile;
            GameObject shot4 = projectile;
            shot1 = Instantiate(shot1, this.transform.position, this.transform.rotation);
            shot2 = Instantiate(shot2, this.transform.position, this.transform.rotation);
            shot3 = Instantiate(shot3, this.transform.position, this.transform.rotation);
            shot4 = Instantiate(shot4, this.transform.position, this.transform.rotation);
            shot1.transform.Rotate(projectile.transform.rotation.x, projectile.transform.rotation.y + 8, projectile.transform.rotation.z);
            shot2.transform.Rotate(projectile.transform.rotation.x, projectile.transform.rotation.y + 3, projectile.transform.rotation.z);
            shot3.transform.Rotate(projectile.transform.rotation.x, projectile.transform.rotation.y - 3, projectile.transform.rotation.z);
            shot4.transform.Rotate(projectile.transform.rotation.x, projectile.transform.rotation.y - 8, projectile.transform.rotation.z);
        }
        else
            {
            GameObject shot1 = projectile;
            GameObject shot2 = projectile;
            GameObject shot3 = projectile;
            GameObject shot4 = projectile;
            shot1 = Instantiate(shot1, this.transform.position, this.transform.rotation);
            shot2 = Instantiate(shot2, this.transform.position, this.transform.rotation);
            Instantiate(projectile, this.transform.position, this.transform.rotation); 
            shot3 = Instantiate(shot3, this.transform.position, this.transform.rotation);
            shot4 = Instantiate(shot4, this.transform.position, this.transform.rotation);
            shot1.transform.Rotate(projectile.transform.rotation.x, projectile.transform.rotation.y + 10, projectile.transform.rotation.z);
            shot2.transform.Rotate(projectile.transform.rotation.x, projectile.transform.rotation.y + 5, projectile.transform.rotation.z);
            shot3.transform.Rotate(projectile.transform.rotation.x, projectile.transform.rotation.y - 5, projectile.transform.rotation.z);
            shot4.transform.Rotate(projectile.transform.rotation.x, projectile.transform.rotation.y - 10, projectile.transform.rotation.z);
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
        if (SaveManager.Instance.saveData.playerSkills > 1)
            currentType++;

        if (SaveManager.Instance.saveData.playerSkills == 3 && (int)currentType >= 3)
            currentType = 0;
        else if (SaveManager.Instance.saveData.playerSkills == 2 && (int)currentType >= 2)
            currentType = 0;
        Debug.Log(currentType);

        switch(currentType)
        {
            case projectileType.fire:
                UIHandler.handler.currentIcon.texture = UIHandler.handler.textureIcons[0];
                break;
            case projectileType.homingFire:
                UIHandler.handler.currentIcon.texture = UIHandler.handler.textureIcons[1];
                break;
            case projectileType.ice:
                UIHandler.handler.currentIcon.texture = UIHandler.handler.textureIcons[2];
                break;

        }
    }
    
}