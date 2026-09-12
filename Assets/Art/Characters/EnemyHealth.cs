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
        EnemyAI victim = gameObject.GetComponent<EnemyAI>();
        if (victim.IsFrozen())
            victim.Unfreeze();
        
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.GetComponent<CapsuleCollider>().isTrigger = false;
        this.GetComponent<CapsuleCollider>().direction = 2;
        //this.GetComponent<CapsuleCollider>().center = new Vector3(0f, 0.4f, 0f);
        
        victim.OnDeath();
        animator.Play("Death");

        if (EnemySpawner.Instance != null)
            EnemySpawner.Instance.DecrementEnemy();
        
        Destroy(this.gameObject, 3.0f);
    }

    public void SetInvulnerable(bool value)
    {
        isInvulnerable = value;
    }

}
