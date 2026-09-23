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
        this.GetComponent<PlayerMovement>().SetWeight(0.0f);
        animator.Play("Hurt");
        this.GetComponent<PlayerMovement>().SetWeight(1.0f);

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
        this.GetComponent<PlayerMovement>().SetWeight(0.0f);
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

        yield return new WaitForSeconds(0.7f);

        animator.SetTrigger("Revive");
        this.GetComponent<PlayerMovement>().SetWeight(1.0f);

        CharacterController controller = this.GetComponent<CharacterController>();
        if (controller != null) // Prevent player hitting the wall or obstacle
        {
            controller.enabled = false;
            SetPlayerTransformation(savePoint);
            controller.enabled = true;
        }

        CombatManager.combatManager.ClearArena();
        
        yield return new WaitForSeconds(1f);
        UIHandler.handler.FadeIn();
        isDefeated = false;
        ResetHealth();
        

        AudioManager.audioManager.PlayTrack(1);
        GameObject npc = GameObject.FindGameObjectWithTag("NPC");
        npc.GetComponent<NPCInteractable>().StartConversationLoss();
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