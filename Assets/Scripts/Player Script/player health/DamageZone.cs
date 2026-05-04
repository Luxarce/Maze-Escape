using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damageAmount = 2f;
    public bool instantKill = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (instantKill)
                PlayerHealth.Instance.TakeDamage(PlayerHealth.Instance.maxHealth);
            else
                PlayerHealth.Instance.TakeDamage(damageAmount);
        }
    }
}
