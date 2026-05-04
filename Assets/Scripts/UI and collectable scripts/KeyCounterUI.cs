using UnityEngine;
using TMPro;

public class KeyCounterUI : MonoBehaviour
{
    public static KeyCounterUI Instance;

    [Header("UI")]
    public TMP_Text counterText;

    private int _collected = 0;
    private int _total = 4; // 4 keys total in the game

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateDisplay();
    }

    public void AddKey()
    {
        _collected++;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (counterText != null)
            counterText.text = "Collected Keys: " + _collected + " / " + _total;
    }
}
