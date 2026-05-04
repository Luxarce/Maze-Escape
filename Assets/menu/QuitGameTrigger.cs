using UnityEngine;

public class QuitGameTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the end! Quitting game...");

#if UNITY_EDITOR
            // Stop play mode in editor
            UnityEditor.EditorApplication.isPlaying = false;
#else
            // Quit the built game
            Application.Quit();
#endif
        }
    }
}