using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Garantir que apenas uma inst�ncia do GameManager exista
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Evita que o GameObject seja destru�do ao carregar uma nova cena
        }
        else
        {
            Destroy(gameObject); // Destr�i objetos duplicados
        }
    }
}
