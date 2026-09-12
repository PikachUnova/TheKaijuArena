using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public CharacterStats stats;
    public float currentHealth;
    public Vector3 savePoint;
    private Animator animator;
    public bool isDefeated = false;

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
        PlayerMovement victim = gameObject.GetComponent<PlayerMovement>();
        if (victim.IsFrozen())
            victim.Unfreeze();
        
        animator.Play("Death");
        isDefeated = true;
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
        SetPlayerTransformation(savePoint);

        yield return new WaitForSeconds(1f);
        UIHandler.handler.FadeIn();
        isDefeated = false;
        ResetHealth();

        AudioManager.audioManager.PlayTrack(1);
        GameObject npc = GameObject.FindGameObjectWithTag("NPC");
        npc.GetComponent<NPCInteractable>().StartCoversationLoss();
    }

    public void ResetHealth()
    {
        currentHealth = stats.maxHealth;
        UIHandler.handler.health = stats.maxHealth;
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