using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple Door Behavior - Handles both true and false doors
/// Attach to each door GameObject
/// </summary>
public class DoorBehavior : MonoBehaviour
{
    [Header("Door Type")]
    [Tooltip("Is this the TRUE door (leads to victory)?")]
    public bool isTrueDoor = false;

    [Header("True Door Settings")]
    [Tooltip("Show victory panel? (if true door)")]
    public GameObject victoryPanel;

    [Tooltip("Name of main menu scene")]
    public string mainMenuSceneName = "MainMenu";

    [Header("False Door Settings")]
    [Tooltip("Kill player when they use false door?")]
    public bool killPlayerOnFalseDoor = true;

    [Tooltip("Message to show when using false door")]
    public string falseDoorMessage = "Wrong door! Try again.";

    [Header("Interaction")]
    [Tooltip("How close player must be to use door")]
    public float interactionDistance = 2f;

    [Tooltip("Prompt to show when near door")]
    public GameObject interactionPrompt;

    // Private variables
    private bool playerNearby = false;
    private GameObject player;

    void Start()
    {
        // Hide victory panel initially
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        // Hide interaction prompt initially
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    void Update()
    {
        // Check if player is nearby
        CheckPlayerProximity();

        // Check for interaction input
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            UseDoor();
        }
    }

    /// <summary>
    /// Checks if player is close enough to interact
    /// </summary>
    void CheckPlayerProximity()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            return;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= interactionDistance)
        {
            if (!playerNearby)
            {
                playerNearby = true;
                ShowPrompt();
            }
        }
        else
        {
            if (playerNearby)
            {
                playerNearby = false;
                HidePrompt();
            }
        }
    }

    /// <summary>
    /// Called when player uses the door
    /// </summary>
    void UseDoor()
    {
        if (isTrueDoor)
        {
            // TRUE DOOR - Show victory!
            OpenTrueDoor();
        }
        else
        {
            // FALSE DOOR - Punish player
            OpenFalseDoor();
        }
    }

    /// <summary>
    /// Opens the true door and shows victory
    /// </summary>
    void OpenTrueDoor()
    {
        Debug.Log("TRUE DOOR! Player wins!");

        // Show victory panel
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);

            // Freeze player
            if (player != null)
            {
                MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in scripts)
                {
                    if (script.GetType().Name.Contains("Controller") ||
                        script.GetType().Name.Contains("Movement"))
                    {
                        script.enabled = false;
                    }
                }
            }

            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // No victory panel, just load main menu
            LoadMainMenu();
        }
    }

    /// <summary>
    /// Opens a false door - kills or restarts player
    /// </summary>
    void OpenFalseDoor()
    {
        Debug.Log($"FALSE DOOR! {falseDoorMessage}");

        if (killPlayerOnFalseDoor)
        {
            // Kill the player
            if (player != null)
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.Die();
                }
            }
        }
        else
        {
            // Just show message
            Debug.Log(falseDoorMessage);
        }
    }

    /// <summary>
    /// Loads the main menu scene
    /// </summary>
    public void LoadMainMenu()
    {
        Debug.Log("Loading main menu...");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    /// <summary>
    /// Shows interaction prompt
    /// </summary>
    void ShowPrompt()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }

    /// <summary>
    /// Hides interaction prompt
    /// </summary>
    void HidePrompt()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    /// <summary>
    /// Trigger-based detection (alternative to distance check)
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
            player = other.gameObject;
            ShowPrompt();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            HidePrompt();
        }
    }

    // Debug visualization
    void OnDrawGizmosSelected()
    {
        // Draw interaction range
        Gizmos.color = isTrueDoor ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}