using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private CharacterStats stats;
    public int currentHealth;

    private Animator animator;
    public EnemyHealthBar healthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth - damage);
        animator.Play("Hurt");

        if (currentHealth <= 0)
        {
            Faint();
        }
    }

    public void Faint()
    {
        animator.Play("Death");
    }



}
