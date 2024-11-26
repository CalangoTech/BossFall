using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // Vida m�xima
    public int currentHealth; // Vida atual

    [Header("UI Settings")]
    public Slider healthSlider; // Barra de vida principal
    public Image healthFill; // Fill da barra de vida principal
    public Slider secondaryHealthSlider; // Barra de vida secund�ria (mais lenta)
    public Image secondaryHealthFill; // Fill da barra de vida secund�ria
    public float secondaryBarSpeed = 2f; // Velocidade de atualiza��o da barra secund�ria

    [Header("Animation Settings")]
    public Animator animator; // Anima��es do jogador

    [Header("Camera Shake Settings")]
    public Camera playerCamera; // C�mera do jogador
    public float shakeMagnitude = 0.1f;
    public float shakeDuration = 0.2f;

    [Header("Damage Feedback Settings")]
    public AudioSource damageAudioSource;
    public AudioClip damageSound;
    public GameObject damageIndicator;
    public float blinkDuration = 1f;

    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        // Atualiza as barras de vida
        UpdateHealthUI();

        // Aciona os efeitos de dano
        if (damageAudioSource != null && damageSound != null)
        {
            damageAudioSource.PlayOneShot(damageSound);
        }
        if (damageIndicator != null)
        {
            StartCoroutine(BlinkDamageIndicator());
        }
        StartCoroutine(CameraShake());

        // Verifica morte
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        // Atualiza a barra principal instantaneamente
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;

            // Controla o estado do Fill da barra principal
            if (healthFill != null)
                healthFill.enabled = currentHealth > 0;
        }

        // Atualiza a barra secund�ria suavemente
        if (secondaryHealthSlider != null)
        {
            StartCoroutine(UpdateSecondaryBar());
        }

        // Verifica se ambas as barras precisam ser ocultadas
        if (currentHealth <= 0)
        {
            HideBothHealthBars();
        }
    }

    private IEnumerator UpdateSecondaryBar()
    {
        if (secondaryHealthSlider != null)
        {
            // Atualiza gradualmente o valor da barra secund�ria
            while (Mathf.Abs(secondaryHealthSlider.value - currentHealth) > 0.01f)
            {
                secondaryHealthSlider.value = Mathf.Lerp(
                    secondaryHealthSlider.value, currentHealth, Time.deltaTime * secondaryBarSpeed);

                yield return null;
            }

            // Garante que o valor final seja exato
            secondaryHealthSlider.value = currentHealth;

            // Controla o estado do Fill da barra secund�ria
            if (secondaryHealthFill != null)
                secondaryHealthFill.enabled = currentHealth > 0;
        }
    }

    void HideBothHealthBars()
    {
        // Desativa o Fill da barra principal
        if (healthFill != null)
            healthFill.enabled = false;

        // Desativa o Fill da barra secund�ria
        if (secondaryHealthFill != null)
            secondaryHealthFill.enabled = false;
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        Debug.Log("Jogador morreu!");
        // Adicione aqui l�gica extra, como reiniciar o jogo ou mostrar Game Over
    }

    private IEnumerator CameraShake()
    {
        Vector3 originalPos = playerCamera.transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            playerCamera.transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.position = originalPos;
    }

    private IEnumerator BlinkDamageIndicator()
    {
        damageIndicator.SetActive(true);
        yield return new WaitForSeconds(blinkDuration);
        damageIndicator.SetActive(false);
    }
}
