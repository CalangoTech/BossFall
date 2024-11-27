using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 100; // Vida máxima do inimigo
    public int currentHealth;  // Vida atual do inimigo

    [Header("Animation Settings")]
    public Animator animator;  // Referência ao Animator
    public bool isDead = false; // Condição de morte para evitar múltiplos acionamentos

    [Header("Destroy Settings")]
    public float destroyDelay = 25f / 60f; // Tempo em segundos baseado nos frames e no FPS

    [Header("Soul Settings")]
    public int minSouls = 10; // Valor mínimo de almas
    public int maxSouls = 50; // Valor máximo de almas

    void Start()
    {
        currentHealth = maxHealth; // Inicializa a vida do inimigo
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

        // Chama o método para dar almas ao jogador
        GiveSouls();

        // Remove o corpo após o delay configurado
        Destroy(gameObject, destroyDelay);
    }

    // Método para dar almas ao jogador
    public void GiveSouls()
    {
        if (!isDead == true)
        {
            int soulsToGive = Random.Range(minSouls, maxSouls + 1); // Sorteia a quantidade de almas
            GameManager.Instance.AddSouls(soulsToGive); // Adiciona as almas no GameManager}

        }
    }
}
