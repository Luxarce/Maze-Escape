using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        // Load the game scene
        SceneManager.LoadScene("Escape Room Game"); // Make sure the name matches exactly
    }

    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit(); // Works when the game is built
    }
}