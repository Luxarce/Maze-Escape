
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    }

    void Update()
    {
        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer <= 0f)
                _isInvincible = false;
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
    }

    public void TakeDamage(float amount)
    {
        if (_isInvincible || _isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player took damage! HP: " + currentHealth);

        _isInvincible = true;
        _invincibleTimer = invincibleDuration;

        UpdateHealthUI();

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    void Die()
    {
        _isDead = true;
        Debug.Log("Player died! Respawning...");
        Respawn();
    }

    public void Respawn()
    {
        _isDead = false;
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (respawnPoint != null && _controller != null)
        {
            _controller.enabled = false;
            transform.position = respawnPoint.position;
            _controller.enabled = true;
        }

        Debug.Log("Player respawned!");
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
    }
}



