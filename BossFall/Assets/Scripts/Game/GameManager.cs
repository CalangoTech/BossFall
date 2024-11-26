using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Garantir que apenas uma instância do GameManager exista
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Evita que o GameObject seja destruído ao carregar uma nova cena
        }
        else
        {
            Destroy(gameObject); // Destrói objetos duplicados
        }
    }
}
