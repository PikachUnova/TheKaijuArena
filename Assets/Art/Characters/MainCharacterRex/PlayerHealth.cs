using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private CharacterStats stats;
    public float currentHealth;

    // Player's starting point
    public Vector3 savePoint = new Vector3(0.0f, 0.5f, 0.0f);

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
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
            Faint();
        }
    }

    public void Faint()
    {
        animator.Play("Death");
        StartCoroutine(Respawn(3f));
    }

     private IEnumerator Respawn(float time)
    {
        yield return new WaitForSeconds(time);

        this.transform.position = savePoint;
    }

}