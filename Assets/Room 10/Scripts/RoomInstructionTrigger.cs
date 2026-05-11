using UnityEngine;

/// <summary>
/// Room Instruction Trigger - Shows instructions when player enters
/// Attach to a trigger collider GameObject at room entrance
/// </summary>
public class RoomInstructionTrigger : MonoBehaviour
{
    [Header("Instruction Settings")]
    [Tooltip("Title of the room")]
    public string roomTitle = "New Room";

    [TextArea(3, 6)]
    [Tooltip("Instructions to show when entering room")]
    public string instructionText = "Find the exit and avoid enemies!";

    [Tooltip("How long to show the instruction (0 = until dismissed)")]
    public float displayDuration = 5f;

    [Header("Trigger Settings")]
    [Tooltip("Only trigger once?")]
    public bool triggerOnce = true;

    [Tooltip("Play sound when triggered?")]
    public AudioClip notificationSound;

    // Private variables
    private bool hasTriggered = false;

    void Start()
    {
        // Ensure this has a trigger collider
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError($"RoomInstructionTrigger on {gameObject.name} needs a Collider component!");
        }
        else if (!col.isTrigger)
        {
            Debug.LogWarning($"Collider on {gameObject.name} should be set to IsTrigger!");
            col.isTrigger = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if player entered
        if (other.CompareTag("Player"))
        {
            // If already triggered and set to trigger once, exit
            if (hasTriggered && triggerOnce)
                return;

            // Show the instruction
            ShowInstruction();

            // Play sound if assigned
            if (notificationSound != null)
            {
                AudioSource.PlayClipAtPoint(notificationSound, transform.position);
            }

            hasTriggered = true;
        }
    }

    /// <summary>
    /// Shows the instruction to the player
    /// </summary>
    void ShowInstruction()
    {
        // Find the instruction UI manager
        InstructionUI instructionUI = FindObjectOfType<InstructionUI>();

        if (instructionUI != null)
        {
            instructionUI.ShowInstruction(roomTitle, instructionText, displayDuration);
        }
        else
        {
            Debug.LogWarning("No InstructionUI found in scene! Add InstructionUI component to a Canvas.");
        }
    }

    /// <summary>
    /// Resets the trigger (useful for testing)
    /// </summary>
    public void ResetTrigger()
    {
        hasTriggered = false;
    }

    // Debug visualization
    void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;

            if (col is BoxCollider)
            {
                BoxCollider box = col as BoxCollider;
                Gizmos.DrawCube(box.center, box.size);
            }
            else if (col is SphereCollider)
            {
                SphereCollider sphere = col as SphereCollider;
                Gizmos.DrawSphere(sphere.center, sphere.radius);
            }
        }
    }
}