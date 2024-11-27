using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int maxHealth = 100; // Vida m�xima do inimigo
    public int currentHealth;  // Vida atual do inimigo

    [Header("Animation Settings")]
    public Animator animator;  // Refer�ncia ao Animator
    public bool isDead = false; // Condi��o de morte para evitar m�ltiplos acionamentos

    [Header("Destroy Settings")]
    public float destroyDelay = 25f / 60f; // Tempo em segundos baseado nos frames e no FPS

    [Header("Soul Settings")]
    public int minSouls = 10; // Valor m�nimo de almas
    public int maxSouls = 50; // Valor m�ximo de almas

    void Start()
    {
        currentHealth = maxHealth; // Inicializa a vida do inimigo
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

        // Chama o m�todo para dar almas ao jogador
        GiveSouls();

        // Remove o corpo ap�s o delay configurado
        Destroy(gameObject, destroyDelay);
    }

    // M�todo para dar almas ao jogador
    public void GiveSouls()
    {
        if (!isDead == true)
        {
            int soulsToGive = Random.Range(minSouls, maxSouls + 1); // Sorteia a quantidade de almas
            GameManager.Instance.AddSouls(soulsToGive); // Adiciona as almas no GameManager}

        }
    }
}
