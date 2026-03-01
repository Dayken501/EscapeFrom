using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UIManager.Instance.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        UIManager.Instance.ShowGameOver();
        Invoke("DisablePlayer", 2f);
    }

    void DisablePlayer()
    {
        gameObject.SetActive(false);
    }
}