using UnityEngine;
using TMPro;

public class KeypadUI : MonoBehaviour
{
    public static KeypadUI Instance;

    public string correctPassword = "1234";
    public GameObject keypadPanel;
    public TMP_Text inputDisplay;
    public TMP_Text feedbackText;
    public ActivationDoor activationDoor;

    private string currentInput = "";
    private bool isOpen = false;
    private int attemptsLeft = 5;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (keypadPanel != null) keypadPanel.SetActive(false);
        if (feedbackText != null) feedbackText.text = "";
    }

    void Update()
    {
        if (!isOpen) return;

        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            else if (c == '\n' || c == '\r')
            {
                Submit();
            }
            else
            {
                currentInput += c;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) CloseKeypad();
        if (inputDisplay != null) inputDisplay.text = currentInput;
    }

    public void OpenKeypad()
    {
        isOpen = true;
        currentInput = "";
        if (keypadPanel != null) keypadPanel.SetActive(true);
        if (feedbackText != null) feedbackText.text = "Attempts left: " + attemptsLeft;
        if (inputDisplay != null) inputDisplay.text = "";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseKeypad()
    {
        isOpen = false;
        currentInput = "";
        if (keypadPanel != null) keypadPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Submit()
    {
        if (currentInput == correctPassword)
        {
            if (feedbackText != null) feedbackText.text = "Correct!";
            Invoke("CloseKeypad", 1f);
            if (activationDoor != null) activationDoor.Unlock();
        }
        else
        {
            attemptsLeft--;
            if (attemptsLeft <= 0)
            {
                if (feedbackText != null) feedbackText.text = "Too many wrong attempts!";
                Invoke("ShowFailMenu", 1f);
            }
            else
            {
                if (feedbackText != null) feedbackText.text = "Wrong! Attempts left: " + attemptsLeft;
                currentInput = "";
            }
        }
    }

    void ShowFailMenu()
    {
        CloseKeypad();
        if (FailMenuUI.Instance != null) FailMenuUI.Instance.Show();
    }
}
