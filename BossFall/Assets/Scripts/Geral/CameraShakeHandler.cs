using UnityEngine;
using System.Collections;

public class CameraShakeHandler : MonoBehaviour
{
    [Header("Camera Settings")]
    public Camera targetCamera; // A c�mera que ser� tremida

    [Header("Shake Type 1 (Weak)")]
    public float shake1Magnitude = 0.1f; // Intensidade do tremor 1
    public float shake1Duration = 0.2f; // Dura��o do tremor 1

    [Header("Shake Type 2 (Strong)")]
    public float shake2Magnitude = 0.3f; // Intensidade do tremor 2
    public float shake2Duration = 0.4f; // Dura��o do tremor 2

    private Vector3 originalPosition; // Posi��o original da c�mera

    void Start()
    {
        // Garante que o script armazene a posi��o original da c�mera
        if (targetCamera != null)
            originalPosition = targetCamera.transform.localPosition;
    }

    /// <summary>
    /// Inicia o tremor tipo 1 (fraco).
    /// </summary>
    public void TriggerShake1()
    {
        TriggerShake(shake1Magnitude, shake1Duration);
    }

    /// <summary>
    /// Inicia o tremor tipo 2 (forte).
    /// </summary>
    public void TriggerShake2()
    {
        TriggerShake(shake2Magnitude, shake2Duration);
    }

    /// <summary>
    /// Fun��o gen�rica para executar o tremor com par�metros personalizados.
    /// </summary>
    private void TriggerShake(float magnitude, float duration)
    {
        if (targetCamera != null)
        {
            StopAllCoroutines(); // Para qualquer tremor anterior
            StartCoroutine(CameraShake(magnitude, duration));
        }
        else
        {
            Debug.LogWarning("Nenhuma c�mera alvo foi atribu�da ao CameraShakeHandler.");
        }
    }

    /// <summary>
    /// Corrotina que executa o tremor da c�mera com os par�metros fornecidos.
    /// </summary>
    private IEnumerator CameraShake(float magnitude, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            targetCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Retorna a c�mera para sua posi��o original ap�s o tremor
        targetCamera.transform.localPosition = originalPosition;
    }
}
