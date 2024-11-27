using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 10; // Dano causado
    public bool applyForce = false; // Se deve aplicar força
    public float forceAmount = 2f;  // Quantidade de força aplicada
    public Vector3 forceDirectionOffset = new Vector3(0, 0.5f, 0); // Direção adicional para ajustar o "pulo"

    [Header("Lifetime Settings")]
    public float lifeTime = 1f; // Tempo antes do objeto desaparecer

    void Start()
    {
        // Destrói o objeto após o tempo definido
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto colidido tem a tag "Enemy"
        if (other.CompareTag("Enemy"))
        {
            // Verifica se o objeto tem o script EnemyBehavior
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);

                // Aplica força se estiver habilitado
                if (applyForce)
                {
                    Rigidbody enemyRb = other.GetComponent<Rigidbody>();
                    if (enemyRb != null)
                    {
                        // Calcula a direção ajustada da força
                        Vector3 forceDirection = (other.transform.position - transform.position).normalized + forceDirectionOffset;

                        // Limita a força aplicada para evitar exageros
                        enemyRb.AddForce(forceDirection * forceAmount, ForceMode.VelocityChange);
                    }
                }
            }
        }
    }
}
