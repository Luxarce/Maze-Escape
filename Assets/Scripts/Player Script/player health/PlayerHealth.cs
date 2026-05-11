using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// DEBUG VERSION - Temporary version with extensive logging
/// Replace your PlayerHealth.cs with this TEMPORARILY to see what's happening
/// After debugging, switch back to your original
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;

    [Header("Health Settings")]
    public float maxHealth = 10f;
    public float currentHealth;

    [Header("UI")]
    public Slider healthBar;
    public TMP_Text healthText;

    [Header("Vignette Effect")]
    public GameObject vignetteObject;
    public float lowHealthThreshold = 4f;
    public float pulseSpeed = 2f;

    [Header("Respawn")]
    public Transform respawnPoint;

    [Header("Invincibility after hit")]
    public float invincibleDuration = 1.5f;
    private float _invincibleTimer = 0f;
    private bool _isInvincible = false;

    private CharacterController _controller;
    private bool _isDead = false;
    private Q_Vignette_Single _vignette;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Debug.Log("=== PLAYERHEALTH: Awake called ===");
    }

    void Start()
    {
        currentHealth = maxHealth;
        _controller = GetComponent<CharacterController>();

        if (vignetteObject != null)
        {
            _vignette = vignetteObject.GetComponent<Q_Vignette_Single>();
            vignetteObject.SetActive(false);
        }

        UpdateHealthUI();

        Debug.Log($"=== PLAYERHEALTH: Start complete ===");
        Debug.Log($"Max Health: {maxHealth}");
        Debug.Log($"Current Health: {currentHealth}");
        Debug.Log($"Invincible Duration: {invincibleDuration}");
    }

    void Update()
    {
        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer <= 0f)
            {
                _isInvincible = false;
                Debug.Log(">>> Invincibility ended");
            }
        }

        // Pulse vignette when low HP
        if (_vignette != null && vignetteObject != null)
        {
            if (currentHealth <= lowHealthThreshold && currentHealth > 0)
            {
                vignetteObject.SetActive(true);
                float alpha = Mathf.Abs(Mathf.Sin(Time.time * pulseSpeed)) * 0.7f + 0.2f;
                _vignette.mainColor = new Color(1f, 0f, 0f, alpha);
            }
            else
            {
                vignetteObject.SetActive(false);
            }
        }

        // DEBUG: Press K to test damage
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log(">>> DEBUG: K pressed - testing TakeDamage(2)");
            TakeDamage(2f);
        }

        // DEBUG: Press L to check status
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("=== PLAYER STATUS ===");
            Debug.Log($"Current Health: {currentHealth}/{maxHealth}");
            Debug.Log($"Is Dead: {_isDead}");
            Debug.Log($"Is Invincible: {_isInvincible}");
            Debug.Log($"Invincible Timer: {_invincibleTimer}");
            Debug.Log("=====================");
        }
    }

    public void TakeDamage(float amount)
    {
        Debug.Log($">>> TakeDamage called! Amount: {amount}");
        Debug.Log($">>> Current state - Health: {currentHealth}, Invincible: {_isInvincible}, Dead: {_isDead}");

        if (_isInvincible)
        {
            Debug.Log(">>> BLOCKED: Player is invincible!");
            return;
        }

        if (_isDead)
        {
            Debug.Log(">>> BLOCKED: Player is already dead!");
            return;
        }

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($">>> DAMAGE APPLIED! New health: {currentHealth}");

        _isInvincible = true;
        _invincibleTimer = invincibleDuration;

        Debug.Log($">>> Invincibility activated for {invincibleDuration} seconds");

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Debug.Log(">>> Health <= 0, calling Die()!");
            Die();
        }
        else
        {
            Debug.Log($">>> Player still alive with {currentHealth} HP");
        }
    }

    public void Heal(float amount)
    {
        Debug.Log($">>> Heal called! Amount: {amount}");
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        Debug.Log($">>> New health: {currentHealth}");
    }

    public void Die()
    {
        Debug.Log(">>> DIE() CALLED!");
        _isDead = true;
        Debug.Log(">>> Player died! Respawning...");
        Respawn();
    }

    public void Respawn()
    {
        Debug.Log(">>> RESPAWN() CALLED!");
        _isDead = false;
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (respawnPoint != null && _controller != null)
        {
            _controller.enabled = false;
            transform.position = respawnPoint.position;
            _controller.enabled = true;
            Debug.Log($">>> Player respawned at {respawnPoint.position}");
        }
        else
        {
            Debug.LogWarning(">>> Respawn failed - missing respawn point or controller!");
        }

        Debug.Log(">>> Player respawned!");
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (healthText != null)
            healthText.text = "HP: " + currentHealth + " / " + maxHealth;

        Debug.Log($">>> UI Updated - Health: {currentHealth}/{maxHealth}");
    }
}