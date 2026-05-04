
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public int checkpointID = 1;

    [Header("Visual - change color when activated")]
    public Renderer plateRenderer;
    public Color inactiveColor = Color.white;
    public Color activeColor = Color.yellow;

    private bool _activated = false;

    void Start()
    {
        if (plateRenderer != null)
            plateRenderer.material.color = inactiveColor;
    }

    void OnTriggerStay(Collider other)
    {
        if (_activated) return;
        if (other.CompareTag("Player"))
        {
            _activated = true;

            if (PlayerHealth.Instance != null)
                PlayerHealth.Instance.respawnPoint = transform;

            if (plateRenderer != null)
                plateRenderer.material.color = activeColor;

            Debug.Log("Checkpoint " + checkpointID + " activated!");
        }
    }
}
