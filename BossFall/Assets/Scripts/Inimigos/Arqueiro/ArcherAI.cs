using UnityEngine;
using UnityEngine.AI;

public class ArcherAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public string playerTag = "Player"; // Tag do jogador para detecção
    private Transform player; // Referência ao Transform do jogador
    private bool isPlayerInRange = false; // Verifica se o jogador está na área

    [Header("Settings")]
    public float safeDistance = 10f; // Distância segura para atirar
    public float shootInterval = 2f; // Intervalo entre disparos

    [Header("Arrow Settings")]
    public GameObject arrowPrefab; // Prefab da flecha
    public Transform arrowSpawnPoint; // Local onde a flecha será instanciada
    public float arrowSpeed = 10f; // Velocidade da flecha

    [Header("Animation Settings")]
    public Animator animator; // Referência ao Animator

    public NavMeshAgent navMeshAgent; // Referência ao NavMeshAgent
    private float nextShootTime; // Controle de tempo para o próximo disparo

    [Header("NavMesh Settings")]
    public float navMeshCheckRadius = 1f; // Raio para alinhamento ao NavMesh
    public float playerDetectionRange = 15f; // Distância máxima de detecção do jogador

    // Referência ao PlayerHealth
    public PlayerHealth playerHealth;

    void Start()
    {
        // Obtém o componente NavMeshAgent
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

        // Alinha o agente ao NavMesh no início
        AlignToNavMesh();
    }

    void Update()
    {
        if (!isPlayerInRange || player == null || navMeshAgent == null || playerHealth.isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Se o jogador não estiver no NavMesh, busque o ponto mais próximo do jogador no NavMesh
        if (!navMeshAgent.isOnNavMesh)
        {
            AlignToNavMesh();
        }

        // Se o jogador estiver muito longe, se aproxima
        if (distanceToPlayer > safeDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            // Se estiver na distância segura, para de mover e atira
            StopMovement();
            ShootAtPlayer();
        }
    }

    void AlignToNavMesh()
    {
        if (player != null && navMeshAgent != null)
        {
            // Verifica se a posição do jogador está dentro do NavMesh
            if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, navMeshCheckRadius, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position); // Define a posição mais próxima no NavMesh
            }
            else
            {
                // Caso o jogador não esteja em uma área do NavMesh, o arqueiro vai para o ponto mais próximo do jogador no NavMesh
                Vector3 targetPosition = player.position;
                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hitPosition, navMeshCheckRadius, NavMesh.AllAreas))
                {
                    navMeshAgent.SetDestination(hitPosition.position);
                    Debug.Log("Jogador não está no NavMesh. O arqueiro se move para o ponto mais próximo.");
                }
            }
        }
    }

    void MoveTowardsPlayer()
    {
        if (!navMeshAgent.isOnNavMesh)
        {
            Debug.LogError("O NavMeshAgent não está em um NavMesh válido!");
            AlignToNavMesh();
            return;
        }

        // Configura o destino do NavMeshAgent para o jogador
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);  // Usa a posição do jogador como o destino no NavMesh

        // Ativa a animação de caminhada
        animator.SetBool("Walk", true);

        // Adiciona rotação para olhar para o jogador enquanto se move
        Vector3 lookDirection = (player.position - transform.position).normalized;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    void StopMovement()
    {
        // Interrompe o movimento ao chegar na distância segura
        if (navMeshAgent.isStopped == false)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath(); // Garante que ele não continua tentando buscar o destino
        }

        // Desativa a animação de caminhada
        animator.SetBool("Walk", false);
    }

    void ShootAtPlayer()
    {
        if (Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootInterval;

            // Ativa o Trigger de animação "Atirar"
            animator.SetTrigger("Atirar");

            // Instancia a flecha após um pequeno delay, sincronizado com a animação
            Invoke(nameof(SpawnArrow), 0.5f); // Ajuste o delay se necessário
        }
    }

    void SpawnArrow()
    {
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            // Instancia a flecha
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);

            // Define a direção para o jogador
            Vector3 direction = (player.position - arrowSpawnPoint.position).normalized;
            direction.y = 0f; // Zera a direção no eixo Y para evitar ângulos verticais

            // Ajusta a rotação da flecha
            arrow.transform.rotation = Quaternion.LookRotation(direction);

            // Aplique a força ou velocidade diretamente na flecha
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * arrowSpeed; // Define a velocidade inicial
            }
        }
    }


    // Detecta o jogador entrando no range do Collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            player = other.transform; // Define o Transform do jogador
            isPlayerInRange = true; // Marca que o jogador está no alcance
            Debug.Log("Jogador detectado pelo arqueiro!");

            // Define a posição do jogador diretamente como destino no NavMesh
            if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, navMeshCheckRadius, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position); // Define a posição mais próxima do jogador dentro do NavMesh
            }
        }
    }

    // Detecta o jogador saindo do range do Collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            player = null; // Remove a referência ao Transform do jogador
            isPlayerInRange = false; // Marca que o jogador está fora do alcance
            Debug.Log("Jogador fora do alcance do arqueiro.");
        }
    }
}
