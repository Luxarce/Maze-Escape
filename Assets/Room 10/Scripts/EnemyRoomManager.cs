using UnityEngine;

/// <summary>
/// Simple Enemy Room Manager - Manages room state and resets
/// Attach to an empty GameObject in the scene
/// </summary>
public class EnemyRoomManager : MonoBehaviour
{
    [Header("Room Setup")]
    [Tooltip("All enemies in this room")]
    public EnemyAI[] enemies;

    [Tooltip("Player GameObject")]
    public GameObject player;

    [Tooltip("Player spawn point")]
    public Transform playerSpawnPoint;

    [Header("Auto-Find Options")]
    [Tooltip("Automatically find all enemies in scene?")]
    public bool autoFindEnemies = true;

    [Tooltip("Automatically find player?")]
    public bool autoFindPlayer = true;

    void Start()
    {
        // Auto-find player
        if (autoFindPlayer && player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                Debug.Log("Auto-found player: " + player.name);
        }

        // Auto-find enemies
        if (autoFindEnemies && (enemies == null || enemies.Length == 0))
        {
            enemies = FindObjectsOfType<EnemyAI>();
            Debug.Log($"Auto-found {enemies.Length} enemies");
        }

        // Validate setup
        if (player == null)
            Debug.LogWarning("Player not assigned! Tag your player as 'Player'");

        if (enemies.Length == 0)
            Debug.LogWarning("No enemies found! Add EnemyAI components to enemies.");
    }

    /// <summary>
    /// Resets the entire room (called when player respawns)
    /// </summary>
    public void ResetRoom()
    {
        Debug.Log("Resetting room...");

        // Reset all enemies
        if (enemies != null)
        {
            foreach (EnemyAI enemy in enemies)
            {
                if (enemy != null)
                    enemy.ResetEnemy();
            }
        }
    }

    /// <summary>
    /// Gets the player GameObject
    /// </summary>
    public GameObject GetPlayer()
    {
        return player;
    }
}