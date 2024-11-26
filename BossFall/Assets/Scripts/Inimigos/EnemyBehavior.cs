using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 100; // Vida m�xima do inimigo
    private int currentHealth;  // Vida atual do inimigo

    [Header("Animation Settings")]
    public Animator animator;  // Refer�ncia ao Animator
    private bool isDead = false; // Condi��o de morte para evitar m�ltiplos acionamentos

    [Header("Destroy Settings")]
    public float destroyDelay = 25f / 60f; // Tempo em segundos baseado nos frames e no FPS

    void Start()
    {
        // Inicializa a vida do inimigo
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Se j� est� morto, n�o processa mais danos

        currentHealth -= damage;

        // Verifica se o inimigo morreu
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true; // Marca que o inimigo est� morto
        animator.SetTrigger("Die"); // Aciona a anima��o de morte

        // Remove o corpo ap�s o delay configurado
        Destroy(gameObject, destroyDelay);
    }
}
