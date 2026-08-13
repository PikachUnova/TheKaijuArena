using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public CharacterStats stats;
    public int currentHealth;

    private Animator animator;
    public EnemyHealthBar healthBar;

    private bool isInvulnerable = false;

    void Start()
    {
        currentHealth = stats.maxHealth;
        healthBar.SetMaxHealth(currentHealth);
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        animator.Play("Hurt");

        if (currentHealth <= 0)
            Faint();
        
    }

    public void Faint()
    {
        animator.Play("Death");
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
    }



}
