using UnityEngine;
using UnityEngine.AI;

public class ArcherAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public string playerTag = "Player"; // Tag do jogador para detec��o
    private Transform player; // Refer�ncia ao Transform do jogador
    private bool isPlayerInRange = false; // Verifica se o jogador est� na �rea

    [Header("Settings")]
    public float safeDistance = 10f; // Dist�ncia segura para atirar
    public float shootInterval = 2f; // Intervalo entre disparos

    [Header("Arrow Settings")]
    public GameObject arrowPrefab; // Prefab da flecha
    public Transform arrowSpawnPoint; // Local onde a flecha ser� instanciada
    public float arrowSpeed = 10f; // Velocidade da flecha

    [Header("Animation Settings")]
    public Animator animator; // Refer�ncia ao Animator

    public NavMeshAgent navMeshAgent; // Refer�ncia ao NavMeshAgent
    private float nextShootTime; // Controle de tempo para o pr�ximo disparo

    [Header("NavMesh Settings")]
    public float navMeshCheckRadius = 1f; // Raio para alinhamento ao NavMesh
    public float playerDetectionRange = 15f; // Dist�ncia m�xima de detec��o do jogador

    // Refer�ncia ao PlayerHealth
    public PlayerHealth playerHealth;

    void Start()
    {
        // Obt�m o componente NavMeshAgent
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

        // Alinha o agente ao NavMesh no in�cio
        AlignToNavMesh();
    }

    void Update()
    {
        if (!isPlayerInRange || player == null || navMeshAgent == null || playerHealth.isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Se o jogador n�o estiver no NavMesh, busque o ponto mais pr�ximo do jogador no NavMesh
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
            // Se estiver na dist�ncia segura, para de mover e atira
            StopMovement();
            ShootAtPlayer();
        }
    }

    void AlignToNavMesh()
    {
        if (player != null && navMeshAgent != null)
        {
            // Verifica se a posi��o do jogador est� dentro do NavMesh
            if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, navMeshCheckRadius, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position); // Define a posi��o mais pr�xima no NavMesh
            }
            else
            {
                // Caso o jogador n�o esteja em uma �rea do NavMesh, o arqueiro vai para o ponto mais pr�ximo do jogador no NavMesh
                Vector3 targetPosition = player.position;
                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hitPosition, navMeshCheckRadius, NavMesh.AllAreas))
                {
                    navMeshAgent.SetDestination(hitPosition.position);
                    Debug.Log("Jogador n�o est� no NavMesh. O arqueiro se move para o ponto mais pr�ximo.");
                }
            }
        }
    }

    void MoveTowardsPlayer()
    {
        if (!navMeshAgent.isOnNavMesh)
        {
            Debug.LogError("O NavMeshAgent n�o est� em um NavMesh v�lido!");
            AlignToNavMesh();
            return;
        }

        // Configura o destino do NavMeshAgent para o jogador
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);  // Usa a posi��o do jogador como o destino no NavMesh

        // Ativa a anima��o de caminhada
        animator.SetBool("Walk", true);

        // Adiciona rota��o para olhar para o jogador enquanto se move
        Vector3 lookDirection = (player.position - transform.position).normalized;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    void StopMovement()
    {
        // Interrompe o movimento ao chegar na dist�ncia segura
        if (navMeshAgent.isStopped == false)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath(); // Garante que ele n�o continua tentando buscar o destino
        }

        // Desativa a anima��o de caminhada
        animator.SetBool("Walk", false);
    }

    void ShootAtPlayer()
    {
        if (Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootInterval;

            // Ativa o Trigger de anima��o "Atirar"
            animator.SetTrigger("Atirar");

            // Instancia a flecha ap�s um pequeno delay, sincronizado com a anima��o
            Invoke(nameof(SpawnArrow), 0.5f); // Ajuste o delay se necess�rio
        }
    }

    void SpawnArrow()
    {
        if (arrowPrefab != null && arrowSpawnPoint != null)
        {
            // Instancia a flecha
            GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);

            // Define a dire��o para o jogador
            Vector3 direction = (player.position - arrowSpawnPoint.position).normalized;
            direction.y = 0f; // Zera a dire��o no eixo Y para evitar �ngulos verticais

            // Ajusta a rota��o da flecha
            arrow.transform.rotation = Quaternion.LookRotation(direction);

            // Aplique a for�a ou velocidade diretamente na flecha
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
            isPlayerInRange = true; // Marca que o jogador est� no alcance
            Debug.Log("Jogador detectado pelo arqueiro!");

            // Define a posi��o do jogador diretamente como destino no NavMesh
            if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, navMeshCheckRadius, NavMesh.AllAreas))
            {
                navMeshAgent.SetDestination(hit.position); // Define a posi��o mais pr�xima do jogador dentro do NavMesh
            }
        }
    }

    // Detecta o jogador saindo do range do Collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            player = null; // Remove a refer�ncia ao Transform do jogador
            isPlayerInRange = false; // Marca que o jogador est� fora do alcance
            Debug.Log("Jogador fora do alcance do arqueiro.");
        }
    }
}
