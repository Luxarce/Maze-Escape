using UnityEngine;
using UnityEngine.SceneManagement;

public class FailMenuUI : MonoBehaviour
{
    public static FailMenuUI Instance;
    public GameObject failPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (failPanel != null) failPanel.SetActive(false);
    }

    public void Show()
    {
        if (failPanel != null) failPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
