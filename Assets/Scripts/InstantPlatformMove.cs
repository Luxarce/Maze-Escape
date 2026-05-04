using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantPlatformMove : MonoBehaviour
{
    public Transform platform;          // The platform to move
    public Vector3 moveOffset;          // How far to move (X, Y, Z)

    private Vector3 startPosition;
    private bool hasMoved = false;

    void Start()
    {
        startPosition = platform.position;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasMoved)
        {
            platform.position += moveOffset; // Instant move
            hasMoved = true;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        platform.position = startPosition; // Reset when scene reloads
        hasMoved = false;
    }
}