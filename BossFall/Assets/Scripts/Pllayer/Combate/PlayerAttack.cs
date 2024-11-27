using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public GameObject heavyAttackIndicator; // Referência ao objeto que indica o cooldown
    public float heavyAttackCooldown = 5f; // Tempo de recarga do ataque pesado
    private float heavyAttackTimer;
    private bool canUseHeavyAttack = true;

    void Update()
    {
        // Ataques normais
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Att1");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("Att2");
        }

        // Ataque pesado
        if (Input.GetKeyDown(KeyCode.Space) && canUseHeavyAttack)
        {
            animator.SetTrigger("HeavyAttack");
            StartCoroutine(StartHeavyAttackCooldown());
        }

        // Atualiza indicador de cooldown
        heavyAttackIndicator.SetActive(canUseHeavyAttack);
    }

    private System.Collections.IEnumerator StartHeavyAttackCooldown()
    {
        canUseHeavyAttack = false;
        yield return new WaitForSeconds(heavyAttackCooldown);
        canUseHeavyAttack = true;
    }
}
