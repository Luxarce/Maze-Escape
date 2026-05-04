using UnityEngine;

public class FallZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player fell off! Respawning...");
            PlayerHealth.Instance.TakeDamage(PlayerHealth.Instance.maxHealth);
        }
    }
}
