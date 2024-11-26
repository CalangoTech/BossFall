using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 100; // Vida máxima do inimigo
    private int currentHealth;  // Vida atual do inimigo

    [Header("Animation Settings")]
    public Animator animator;  // Referência ao Animator
    private bool isDead = false; // Condição de morte para evitar múltiplos acionamentos

    [Header("Destroy Settings")]
    public float destroyDelay = 25f / 60f; // Tempo em segundos baseado nos frames e no FPS

    void Start()
    {
        // Inicializa a vida do inimigo
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Se já está morto, não processa mais danos

        currentHealth -= damage;

        // Verifica se o inimigo morreu
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true; // Marca que o inimigo está morto
        animator.SetTrigger("Die"); // Aciona a animação de morte

        // Remove o corpo após o delay configurado
        Destroy(gameObject, destroyDelay);
    }
}
