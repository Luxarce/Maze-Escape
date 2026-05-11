using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// DEBUG Door Behavior - Extensive logging to find victory panel issues
/// Replace your DoorBehavior.cs TEMPORARILY with this version
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
    [Tooltip("Damage player when they use false door?")]
    public bool damagePlayerOnFalseDoor = true;

    [Tooltip("How much damage to deal (set high to kill instantly)")]
    public float falseDoorDamage = 100f;

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
        Debug.Log($"=== DOOR: {gameObject.name} initialized ===");
        Debug.Log($">>> Is True Door: {isTrueDoor}");
        Debug.Log($">>> Victory Panel: {(victoryPanel != null ? victoryPanel.name : "NULL")}");
        Debug.Log($">>> Victory Panel Active: {(victoryPanel != null ? victoryPanel.activeSelf : false)}");

        // Hide victory panel initially
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
            Debug.Log(">>> Victory panel set to inactive");
        }

        // Hide interaction prompt initially
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        // Check for Box Collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Debug.Log($">>> Has collider: {col.GetType().Name}");
            Debug.Log($">>> Is Trigger: {col.isTrigger}");
        }
        else
        {
            Debug.LogError(">>> NO COLLIDER FOUND!");
        }
    }

    void Update()
    {
        // Check if player is nearby
        CheckPlayerProximity();

        // Check for interaction input
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($">>> E pressed near door: {gameObject.name}");
            UseDoor();
        }

        // DEBUG: Press O to test opening this door
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log($">>> DEBUG: O pressed - testing door {gameObject.name}");
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
            if (player != null)
            {
                Debug.Log($">>> Found player: {player.name}");
            }
            return;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance <= interactionDistance)
        {
            if (!playerNearby)
            {
                playerNearby = true;
                Debug.Log($">>> Player entered range of {gameObject.name}");
                ShowPrompt();
            }
        }
        else
        {
            if (playerNearby)
            {
                playerNearby = false;
                Debug.Log($">>> Player left range of {gameObject.name}");
                HidePrompt();
            }
        }
    }

    /// <summary>
    /// Called when player uses the door
    /// </summary>
    void UseDoor()
    {
        Debug.Log(">>> ===================================");
        Debug.Log($">>> UseDoor() called on: {gameObject.name}");
        Debug.Log($">>> isTrueDoor: {isTrueDoor}");
        Debug.Log(">>> ===================================");

        if (isTrueDoor)
        {
            Debug.Log(">>> ROUTING TO: OpenTrueDoor()");
            OpenTrueDoor();
        }
        else
        {
            Debug.Log(">>> ROUTING TO: OpenFalseDoor()");
            OpenFalseDoor();
        }
    }

    /// <summary>
    /// Opens the true door and shows victory
    /// </summary>
    void OpenTrueDoor()
    {
        Debug.Log(">>> ===================================");
        Debug.Log(">>> OpenTrueDoor() CALLED!");
        Debug.Log(">>> TRUE DOOR! Player wins!");
        Debug.Log(">>> ===================================");

        // Check victory panel
        if (victoryPanel != null)
        {
            Debug.Log($">>> Victory Panel found: {victoryPanel.name}");
            Debug.Log($">>> Victory Panel current state: {victoryPanel.activeSelf}");
            Debug.Log($">>> Setting Victory Panel to ACTIVE...");

            victoryPanel.SetActive(true);

            Debug.Log($">>> Victory Panel new state: {victoryPanel.activeSelf}");

            // Check if it has VictoryPanel script
            VictoryPanel vpScript = victoryPanel.GetComponent<VictoryPanel>();
            if (vpScript != null)
            {
                Debug.Log(">>> VictoryPanel script found!");
            }
            else
            {
                Debug.LogWarning(">>> VictoryPanel script NOT found!");
            }

            // Freeze player
            if (player != null)
            {
                Debug.Log(">>> Attempting to freeze player controls...");
                MonoBehaviour[] scripts = player.GetComponents<MonoBehaviour>();
                int frozenCount = 0;

                foreach (MonoBehaviour script in scripts)
                {
                    if (script.GetType().Name.Contains("Controller") ||
                        script.GetType().Name.Contains("Movement"))
                    {
                        script.enabled = false;
                        frozenCount++;
                        Debug.Log($">>> Disabled: {script.GetType().Name}");
                    }
                }

                Debug.Log($">>> Froze {frozenCount} player scripts");
            }
            else
            {
                Debug.LogWarning(">>> Player is NULL!");
            }

            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log(">>> Cursor unlocked and visible");
        }
        else
        {
            Debug.LogError(">>> VICTORY PANEL IS NULL!");
            Debug.LogError(">>> Assign Victory Panel in Inspector!");
            Debug.Log(">>> Attempting to load main menu as fallback...");
            LoadMainMenu();
        }
    }

    /// <summary>
    /// Opens a false door - damages player
    /// </summary>
    void OpenFalseDoor()
    {
        Debug.Log(">>> ===================================");
        Debug.Log(">>> OpenFalseDoor() CALLED!");
        Debug.Log($">>> FALSE DOOR! {falseDoorMessage}");
        Debug.Log(">>> ===================================");

        if (damagePlayerOnFalseDoor)
        {
            Debug.Log(">>> Damage enabled for false door");

            if (player != null)
            {
                Debug.Log($">>> Player found: {player.name}");

                PlayerHealth health = player.GetComponent<PlayerHealth>();

                if (health != null)
                {
                    Debug.Log($">>> Dealing {falseDoorDamage} damage!");
                    health.TakeDamage(falseDoorDamage);
                }
                else
                {
                    Debug.LogError(">>> PlayerHealth component NOT FOUND!");
                }
            }
            else
            {
                Debug.LogError(">>> Player is NULL!");
            }
        }
        else
        {
            Debug.Log(">>> Damage is DISABLED for this false door");
        }
    }

    /// <summary>
    /// Loads the main menu scene
    /// </summary>
    public void LoadMainMenu()
    {
        Debug.Log($">>> Loading main menu scene: {mainMenuSceneName}");
        SceneManager.LoadScene(mainMenuSceneName);
    }

    /// <summary>
    /// Shows interaction prompt
    /// </summary>
    void ShowPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
            Debug.Log(">>> Interaction prompt shown");
        }
    }

    /// <summary>
    /// Hides interaction prompt
    /// </summary>
    void HidePrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
            Debug.Log(">>> Interaction prompt hidden");
        }
    }

    /// <summary>
    /// Trigger-based detection
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($">>> Trigger entered by: {other.gameObject.name} (Tag: {other.tag})");

        if (other.CompareTag("Player"))
        {
            Debug.Log($">>> Player entered trigger for {gameObject.name}");
            playerNearby = true;
            player = other.gameObject;
            ShowPrompt();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($">>> Player exited trigger for {gameObject.name}");
            playerNearby = false;
            HidePrompt();
        }
    }

    // Debug visualization
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isTrueDoor ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);

        // Draw a label
        if (isTrueDoor)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position + Vector3.up * 2, Vector3.one * 0.5f);
        }
    }
}