using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public CharacterStats stats;
    public float currentHealth;

    public Vector3 savePoint;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = stats.maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        UIHandler.handler.health -= damage;
        currentHealth -= damage;
        animator.Play("Hurt");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            UIHandler.handler.health = 0;
            Faint();
        }
    }

    public void Faint()
    {
        animator.Play("Death");
        this.GetComponent<PlayerMovement>().canMove = false;
        StartCoroutine(Respawn());
    }



    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f);
        UIHandler.handler.FadeOut();

        yield return new WaitForSeconds(0.5f);

        animator.SetTrigger("Revive");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetPlayerTransformation(savePoint);

        yield return new WaitForSeconds(1.2f);
        UIHandler.handler.FadeIn();

        currentHealth = stats.maxHealth;
        UIHandler.handler.health = stats.maxHealth;
        this.GetComponent<PlayerMovement>().canMove = true;

    }

    public void SetPlayerTransformation(Vector3 point)
    {
        this.transform.position = point;
    }


}