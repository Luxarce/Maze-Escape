using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [Header("Medkit Settings")]
    public float healAmount = 40f;
    public int maxMedkits = 3;

    [Header("UI")]
    public GameObject medkitSlot;
    public TMP_Text medkitCountText;
    public Image medkitImage;

    private int _medkitCount = 0;
    private PlayerHealth _playerHealth;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Fixed for Unity 6
        _playerHealth = Object.FindFirstObjectByType<PlayerHealth>();
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UseMedKit();
    }

    public void AddMedKit()
    {
        if (_medkitCount < maxMedkits)
        {
            _medkitCount++;
            UpdateUI();
            Debug.Log("Medkit added. Total: " + _medkitCount);
        }
        else
        {
            Debug.Log("Medkit inventory full!");
        }
    }

    void UseMedKit()
    {
        if (_medkitCount <= 0)
        {
            Debug.Log("No medkits left!");
            return;
        }

        if (_playerHealth.currentHealth >= _playerHealth.maxHealth)
        {
            Debug.Log("Already at full health!");
            return;
        }

        _medkitCount--;
        _playerHealth.Heal(healAmount);
        UpdateUI();
        Debug.Log("Used medkit. Healed: " + healAmount + " | Medkits left: " + _medkitCount);
    }

    void UpdateUI()
    {
        if (medkitCountText != null)
            medkitCountText.text = "x" + _medkitCount;
        if (medkitSlot != null)
            medkitSlot.SetActive(_medkitCount > 0);
    }
}
