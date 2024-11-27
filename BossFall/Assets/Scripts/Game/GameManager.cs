using UnityEngine;
using TMPro; // Adicionei o namespace para o TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int soulCount = 0; // Variável para armazenar a quantidade de almas
    public TextMeshProUGUI soulText; // Referência ao TextMeshPro para atualizar o HUD

    EnemyBehavior enemyBehavior;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Atualiza o TextMeshPro na HUD com a quantidade de almas
        if (soulText != null)
        {
            soulText.text = "" + soulCount.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto com a tag "Enemy" entrou no trigger
        if (other.CompareTag("Enemy"))
        {
            // Adiciona as almas do inimigo ao jogador
            EnemyBehavior enemy = other.GetComponent<EnemyBehavior>();
            if (enemy != null)
            {
                enemy.GiveSouls();
            }
        }
    }

    // Método para adicionar almas
    public void AddSouls(int amount)
    {
        soulCount += amount;
    }
}
