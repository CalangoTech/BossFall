using UnityEngine;

public class DamageObject : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 10; // Dano causado
    public bool applyForce = false; // Se deve aplicar for�a
    public float forceAmount = 2f;  // Quantidade de for�a aplicada
    public Vector3 forceDirectionOffset = new Vector3(0, 0.5f, 0); // Dire��o adicional para ajustar o "pulo"

    [Header("Lifetime Settings")]
    public float lifeTime = 1f; // Tempo antes do objeto desaparecer

    void Start()
    {
        // Destr�i o objeto ap�s o tempo definido
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

                // Aplica for�a se estiver habilitado
                if (applyForce)
                {
                    Rigidbody enemyRb = other.GetComponent<Rigidbody>();
                    if (enemyRb != null)
                    {
                        // Calcula a dire��o ajustada da for�a
                        Vector3 forceDirection = (other.transform.position - transform.position).normalized + forceDirectionOffset;

                        // Limita a for�a aplicada para evitar exageros
                        enemyRb.AddForce(forceDirection * forceAmount, ForceMode.VelocityChange);
                    }
                }
            }
        }
    }
}
