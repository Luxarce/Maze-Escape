using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Simple Victory Panel - Shows when player wins
/// Attach to the Victory Panel UI GameObject
/// </summary>
public class VictoryPanel : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Name of main menu scene")]
    public string mainMenuSceneName = "MainMenu";

    [Tooltip("Name of current level (to replay)")]
    public string currentLevelName;

    [Header("UI Elements (Optional)")]
    [Tooltip("Victory text to show")]
    public UnityEngine.UI.Text victoryText;

    [Tooltip("Message to display")]
    public string victoryMessage = "YOU WIN!";

    void Start()
    {
        // Get current scene name if not set
        if (string.IsNullOrEmpty(currentLevelName))
        {
            currentLevelName = SceneManager.GetActiveScene().name;
        }

        // Set victory message
        if (victoryText != null)
        {
            victoryText.text = victoryMessage;
        }

        // Unlock cursor for UI interaction
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// Called by Main Menu button
    /// </summary>
    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu...");
        Time.timeScale = 1f; // Reset time in case it was paused
        SceneManager.LoadScene(mainMenuSceneName);
    }

    /// <summary>
    /// Called by Replay button (optional)
    /// </summary>
    public void ReplayLevel()
    {
        Debug.Log("Replaying level...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(currentLevelName);
    }

    /// <summary>
    /// Called by Quit button (optional)
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}