using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public CharacterStats stats;
    public float currentHealth;

    public Vector3 savePoint;

    private Animator animator;

    void Start()
    {
        currentHealth = stats.maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
            return;

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

    private void Faint()
    {
        animator.Play("Death");
        this.GetComponent<PlayerMovement>().canMove = false;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnTime());
    }

    private IEnumerator RespawnTime()
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

        GameObject npc = GameObject.FindGameObjectWithTag("NPC");
        npc.GetComponent<NPCInteractable>().StartCoversationLoss();
    }

    public void SetPlayerTransformation(Vector3 point)
    {
        this.transform.position = point;
    }

    public bool IsDefeated()
    {
        if (currentHealth <= 0) return true;
        return false;
    }

}