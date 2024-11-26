using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    public int damage = 20; // Dano causado pela flecha
    public float lifetime = 5f; // Tempo para destruir a flecha caso n�o acerte nada
    public float speed = 10f; // Velocidade da flecha

    private Rigidbody rb;

    void Start()
    {
        // Destr�i a flecha automaticamente ap�s o tempo definido
        Destroy(gameObject, lifetime);

        // Obt�m o Rigidbody e aplica o movimento para frente
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false; // Desativa a gravidade para a flecha

            // Move a flecha para frente, no eixo Y, mas com rota��o ajustada em 90 graus no eixo X
            rb.velocity = transform.forward * speed; // Move a flecha para frente

            // Rotaciona a flecha em 90 graus no eixo X
            transform.rotation = Quaternion.Euler(90f, transform.eulerAngles.y, transform.eulerAngles.z);
        }
        else
        {
            Debug.LogError("Rigidbody n�o encontrado na flecha!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se a flecha acertou o jogador
        if (other.CompareTag("Player"))
        {
            // Obt�m o componente PlayerHealth para aplicar o dano
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); // Aplica o dano
            }

            // Destroi a flecha ao atingir o jogador
            Destroy(gameObject);
        }
    }
}
