using UnityEngine;

/// <summary>
/// UPDATED EnemyAI.cs - Works with your existing PlayerHealth script
/// Replace your current EnemyAI.cs with this version
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("Points the enemy will patrol between")]
    public Transform[] patrolPoints;

    [Tooltip("How fast the enemy moves while patrolling")]
    public float patrolSpeed = 2f;

    [Tooltip("How close to waypoint before moving to next")]
    public float waypointReachDistance = 0.5f;

    [Header("Chase Settings")]
    [Tooltip("How fast the enemy moves while chasing")]
    public float chaseSpeed = 4f;

    [Tooltip("How far the enemy can see the player")]
    public float detectionRange = 10f;

    [Tooltip("Field of view angle (180 = see forward half)")]
    public float fieldOfView = 120f;

    [Header("Combat Settings")]
    [Tooltip("Damage dealt to player on collision")]
    public float damageAmount = 2f;

    [Tooltip("Time between damage hits")]
    public float damageInterval = 1f;

    [Header("References")]
    [Tooltip("The player Transform (auto-finds if empty)")]
    public Transform player;

    [Tooltip("Layer mask for obstacles (walls)")]
    public LayerMask obstacleLayer;

    // Private variables
    private int currentWaypoint = 0;
    private bool isChasing = false;
    private Vector3 startPosition;
    private float lastDamageTime = 0f;

    void Start()
    {
        startPosition = transform.position;

        // Auto-find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // Validate setup
        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning($"Enemy {name} has no patrol points! Add waypoints.");
        }
    }

    void Update()
    {
        // Check if player is visible
        CheckForPlayer();

        // Move based on state
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    /// <summary>
    /// Checks if player is in sight
    /// </summary>
    void CheckForPlayer()
    {
        if (player == null) return;

        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Too far away
        if (distanceToPlayer > detectionRange)
        {
            isChasing = false;
            return;
        }

        // Check if player is in field of view
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer > fieldOfView / 2f)
        {
            isChasing = false;
            return;
        }

        // Check line of sight (no walls blocking)
        if (Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer))
        {
            // Wall is blocking view
            isChasing = false;
            return;
        }

        // Player is visible!
        isChasing = true;
    }

    /// <summary>
    /// Patrols between waypoints
    /// </summary>
    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        // Move toward current waypoint
        Transform targetWaypoint = patrolPoints[currentWaypoint];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * patrolSpeed * Time.deltaTime;

        // Look in movement direction
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        // Check if reached waypoint
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        if (distance < waypointReachDistance)
        {
            // Move to next waypoint
            currentWaypoint = (currentWaypoint + 1) % patrolPoints.Length;
        }
    }

    /// <summary>
    /// Chases the player
    /// </summary>
    void ChasePlayer()
    {
        if (player == null) return;

        // Move toward player
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;

        // Look at player
        transform.rotation = Quaternion.LookRotation(direction);
    }

    /// <summary>
    /// Resets enemy to starting position
    /// </summary>
    public void ResetEnemy()
    {
        transform.position = startPosition;
        isChasing = false;
        currentWaypoint = 0;
    }

    /// <summary>
    /// Called when player collides with enemy
    /// UPDATED: Uses TakeDamage instead of Die()
    /// </summary>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Deal damage to player using their existing health system
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Check if enough time has passed since last damage
                if (Time.time - lastDamageTime >= damageInterval)
                {
                    playerHealth.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;
                    Debug.Log($"Enemy dealt {damageAmount} damage to player!");
                }
            }
        }
    }

    /// <summary>
    /// UPDATED: Continuous damage while touching
    /// </summary>
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                if (Time.time - lastDamageTime >= damageInterval)
                {
                    playerHealth.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    // Debug visualization
    void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw field of view
        Vector3 fovLine1 = Quaternion.AngleAxis(fieldOfView / 2f, transform.up) * transform.forward * detectionRange;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fieldOfView / 2f, transform.up) * transform.forward * detectionRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + fovLine1);
        Gizmos.DrawLine(transform.position, transform.position + fovLine2);

        // Draw patrol path
        if (patrolPoints != null && patrolPoints.Length > 1)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    int nextIndex = (i + 1) % patrolPoints.Length;
                    if (patrolPoints[nextIndex] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[nextIndex].position);
                    }
                }
            }
        }
    }
}