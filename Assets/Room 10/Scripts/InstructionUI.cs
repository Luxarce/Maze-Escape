using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Instruction UI Manager - Displays room instructions
/// Attach to a UI Panel GameObject
/// </summary>
public class InstructionUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Title text component")]
    public TMP_Text titleText;

    [Tooltip("Instruction text component")]
    public TMP_Text instructionText;

    [Tooltip("Panel background")]
    public GameObject panel;

    [Header("Animation Settings")]
    [Tooltip("Fade in duration")]
    public float fadeInDuration = 0.5f;

    [Tooltip("Fade out duration")]
    public float fadeOutDuration = 0.5f;

    [Header("Dismiss Settings")]
    [Tooltip("Can player dismiss with key press?")]
    public bool allowDismiss = true;

    [Tooltip("Key to dismiss instruction")]
    public KeyCode dismissKey = KeyCode.Space;

    [Tooltip("Show dismiss prompt?")]
    public TMP_Text dismissPromptText;

    // Private variables
    private CanvasGroup canvasGroup;
    private float displayTimer = 0f;
    private float displayDuration = 0f;
    private bool isShowing = false;
    private bool isFading = false;

    void Start()
    {
        // Get or add CanvasGroup for fading
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Start hidden
        HideImmediate();

        // Setup dismiss prompt
        if (dismissPromptText != null)
        {
            dismissPromptText.text = $"Press {dismissKey} to continue";
        }
    }

    void Update()
    {
        if (!isShowing) return;

        // Handle dismiss input
        if (allowDismiss && Input.GetKeyDown(dismissKey))
        {
            HideInstruction();
            return;
        }

        // Handle auto-hide timer
        if (displayDuration > 0)
        {
            displayTimer -= Time.deltaTime;
            if (displayTimer <= 0f)
            {
                HideInstruction();
            }
        }
    }

    /// <summary>
    /// Shows an instruction to the player
    /// </summary>
    /// <param name="title">Room title</param>
    /// <param name="instruction">Instruction text</param>
    /// <param name="duration">How long to show (0 = until dismissed)</param>
    public void ShowInstruction(string title, string instruction, float duration = 0f)
    {
        // Set text
        if (titleText != null)
            titleText.text = title;

        if (instructionText != null)
            instructionText.text = instruction;

        // Set duration
        displayDuration = duration;
        displayTimer = duration;

        // Show panel
        if (panel != null)
            panel.SetActive(true);

        // Show dismiss prompt if duration is 0
        if (dismissPromptText != null)
        {
            dismissPromptText.gameObject.SetActive(duration <= 0 && allowDismiss);
        }

        // Fade in
        StopAllCoroutines();
        StartCoroutine(FadeIn());

        isShowing = true;
    }

    /// <summary>
    /// Hides the instruction
    /// </summary>
    public void HideInstruction()
    {
        if (!isShowing) return;

        StopAllCoroutines();
        StartCoroutine(FadeOut());

        isShowing = false;
    }

    /// <summary>
    /// Immediately hides without fade
    /// </summary>
    void HideImmediate()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }

        if (panel != null)
            panel.SetActive(false);

        isShowing = false;
    }

    /// <summary>
    /// Fades the instruction in
    /// </summary>
    System.Collections.IEnumerator FadeIn()
    {
        isFading = true;
        float elapsed = 0f;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeInDuration);

            if (canvasGroup != null)
                canvasGroup.alpha = alpha;

            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 1f;

        isFading = false;
    }

    /// <summary>
    /// Fades the instruction out
    /// </summary>
    System.Collections.IEnumerator FadeOut()
    {
        isFading = true;
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(elapsed / fadeOutDuration);

            if (canvasGroup != null)
                canvasGroup.alpha = alpha;

            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        if (panel != null)
            panel.SetActive(false);

        isFading = false;
    }

    /// <summary>
    /// Check if instruction is currently showing
    /// </summary>
    public bool IsShowing()
    {
        return isShowing;
    }
}