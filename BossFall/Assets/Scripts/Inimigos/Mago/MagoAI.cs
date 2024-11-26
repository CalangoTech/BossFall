using UnityEngine;
using UnityEngine.AI;

public class MagoAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public string playerTag = "Player"; // Tag do jogador para detec��o
    private Transform player; // Refer�ncia ao Transform do jogador
    private bool isPlayerInRange = false; // Verifica se o jogador est� na �rea

    [Header("Settings")]
    public float safeDistance = 12f; // Dist�ncia segura para lan�ar magias
    public float spellInterval = 3f; // Intervalo entre lan�amentos de magia

    [Header("Fireball Settings")]
    public GameObject fireballPrefab; // Prefab da bola de fogo
    public Transform fireballSpawnPoint; // Local onde a bola de fogo ser� instanciada
    public float fireballSpeed = 15f; // Velocidade da bola de fogo

    [Header("Animation Settings")]
    public Animator animator; // Refer�ncia ao Animator

    public NavMeshAgent navMeshAgent; // Refer�ncia ao NavMeshAgent
    private float nextSpellTime; // Controle de tempo para o pr�ximo lan�amento de magia

    [Header("NavMesh Settings")]
    public float navMeshCheckRadius = 1f; // Raio para alinhamento ao NavMesh
    public float playerDetectionRange = 18f; // Dist�ncia m�xima de detec��o do jogador

    // Refer�ncia ao PlayerHealth
    public PlayerHealth playerHealth;

    void Start()
    {
        // Obt�m o NavMeshAgent e verifica sua exist�ncia
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent n�o encontrado no GameObject!");
        }

        // Obt�m a refer�ncia do PlayerHealth
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
            // Se estiver na dist�ncia segura, para de mover e lan�a magia
            StopMovement();
            CastSpell();
        }
    }

    void MoveTowardsPlayer()
    {
        if (!navMeshAgent.isOnNavMesh)
        {
            Debug.LogError("O NavMeshAgent n�o est� em um NavMesh v�lido!");
            return;
        }

        // Configura o destino do NavMeshAgent para o jogador
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);

        // Ativa a anima��o de caminhada
        animator.SetBool("Walk", true);

        // Adiciona rota��o para olhar para o jogador enquanto se move
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    void StopMovement()
    {
        // Interrompe o movimento ao chegar na dist�ncia segura
        if (!navMeshAgent.isStopped)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath(); // Garante que ele n�o continua tentando buscar o destino
        }

        // Desativa a anima��o de caminhada
        animator.SetBool("Walk", false);
    }

    void CastSpell()
    {
        if (Time.time >= nextSpellTime)
        {
            nextSpellTime = Time.time + spellInterval;

            // Ativa o Trigger de anima��o "Lan�arMagia"
            animator.SetTrigger("Lan�arMagia");

            // Instancia a bola de fogo ap�s um pequeno delay, sincronizado com a anima��o
            Invoke(nameof(SpawnFireball), 0.5f); // Ajuste o delay se necess�rio
        }
    }

    void SpawnFireball()
    {
        if (fireballPrefab != null && fireballSpawnPoint != null)
        {
            // Instancia a bola de fogo
            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);

            // Define a dire��o para o jogador
            Vector3 direction = (player.position - fireballSpawnPoint.position).normalized;
            direction.y = 0f; // Zera a dire��o no eixo Y para evitar �ngulos verticais

            // Ajusta a rota��o da bola de fogo
            fireball.transform.rotation = Quaternion.LookRotation(direction);

            // Aplica a velocidade � bola de fogo
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
            isPlayerInRange = true; // Marca que o jogador est� no alcance
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            player = null; // Remove a refer�ncia ao Transform do jogador
            isPlayerInRange = false; // Marca que o jogador est� fora do alcance
        }
    }
}
