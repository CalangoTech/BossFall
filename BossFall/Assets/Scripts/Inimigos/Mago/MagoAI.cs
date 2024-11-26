using UnityEngine;
using UnityEngine.AI;

public class MagoAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public string playerTag = "Player"; // Tag do jogador para detecção
    private Transform player; // Referência ao Transform do jogador
    private bool isPlayerInRange = false; // Verifica se o jogador está na área

    [Header("Settings")]
    public float safeDistance = 12f; // Distância segura para lançar magias
    public float spellInterval = 3f; // Intervalo entre lançamentos de magia

    [Header("Fireball Settings")]
    public GameObject fireballPrefab; // Prefab da bola de fogo
    public Transform fireballSpawnPoint; // Local onde a bola de fogo será instanciada
    public float fireballSpeed = 15f; // Velocidade da bola de fogo

    [Header("Animation Settings")]
    public Animator animator; // Referência ao Animator

    public NavMeshAgent navMeshAgent; // Referência ao NavMeshAgent
    private float nextSpellTime; // Controle de tempo para o próximo lançamento de magia

    [Header("NavMesh Settings")]
    public float navMeshCheckRadius = 1f; // Raio para alinhamento ao NavMesh
    public float playerDetectionRange = 18f; // Distância máxima de detecção do jogador

    // Referência ao PlayerHealth
    public PlayerHealth playerHealth;

    void Start()
    {
        // Obtém o NavMeshAgent e verifica sua existência
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent não encontrado no GameObject!");
        }

        // Obtém a referência do PlayerHealth
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<PlayerHealth>();
        }
    }

    void Update()
    {
        if (!isPlayerInRange || player == null || navMeshAgent == null || playerHealth.isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Se o jogador estiver muito longe, se aproxima
        if (distanceToPlayer > safeDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            // Se estiver na distância segura, para de mover e lança magia
            StopMovement();
            CastSpell();
        }
    }

    void MoveTowardsPlayer()
    {
        if (!navMeshAgent.isOnNavMesh)
        {
            Debug.LogError("O NavMeshAgent não está em um NavMesh válido!");
            return;
        }

        // Configura o destino do NavMeshAgent para o jogador
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);

        // Ativa a animação de caminhada
        animator.SetBool("Walk", true);

        // Adiciona rotação para olhar para o jogador enquanto se move
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    void StopMovement()
    {
        // Interrompe o movimento ao chegar na distância segura
        if (!navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath(); // Garante que ele não continua tentando buscar o destino
        }

        // Desativa a animação de caminhada
        animator.SetBool("Walk", false);
    }

    void CastSpell()
    {
        if (Time.time >= nextSpellTime)
        {
            nextSpellTime = Time.time + spellInterval;

            // Ativa o Trigger de animação "LançarMagia"
            animator.SetTrigger("LançarMagia");

            // Instancia a bola de fogo após um pequeno delay, sincronizado com a animação
            Invoke(nameof(SpawnFireball), 0.5f); // Ajuste o delay se necessário
        }
    }

    void SpawnFireball()
    {
        if (fireballPrefab != null && fireballSpawnPoint != null)
        {
            // Instancia a bola de fogo
            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);

            // Define a direção para o jogador
            Vector3 direction = (player.position - fireballSpawnPoint.position).normalized;
            direction.y = 0f; // Zera a direção no eixo Y para evitar ângulos verticais

            // Ajusta a rotação da bola de fogo
            fireball.transform.rotation = Quaternion.LookRotation(direction);

            // Aplica a velocidade à bola de fogo
            Rigidbody rb = fireball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * fireballSpeed; // Define a velocidade inicial
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            player = other.transform; // Define o Transform do jogador
            isPlayerInRange = true; // Marca que o jogador está no alcance
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            player = null; // Remove a referência ao Transform do jogador
            isPlayerInRange = false; // Marca que o jogador está fora do alcance
        }
    }
}
