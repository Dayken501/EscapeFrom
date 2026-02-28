using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GetComponent<EnemyAI>().enabled = false;
        UIManager.Instance.AddKill();
        GameManager.Instance.EnemyDied();
        Destroy(gameObject, 2f);
    }
}