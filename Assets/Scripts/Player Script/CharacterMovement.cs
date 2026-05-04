using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float gravity = -19.62f;

    [Header("Jump Settings")]
    public float jumpHeight = 1.8f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private CharacterController _cc;
    private Vector3 _velocity;
    private bool _isGrounded;

    private bool _isDashing = false;
    private float _dashTimer = 0f;
    private float _dashCooldownTimer = 0f;
    private Vector3 _dashDirection;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        GroundCheck();

        if (_isDashing)
        {
            HandleDash();
            return;
        }

        HandleMovement();
        HandleJump();
        ApplyGravity();
        TryStartDash();

        if (_dashCooldownTimer > 0f)
            _dashCooldownTimer -= Time.deltaTime;
    }

    void GroundCheck()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (_isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 direction = transform.right * h + transform.forward * v;

        _cc.Move(direction * walkSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    void ApplyGravity()
    {
        _velocity.y += gravity * Time.deltaTime;
        _cc.Move(_velocity * Time.deltaTime);
    }

    void TryStartDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashCooldownTimer <= 0f)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 inputDir = transform.right * h + transform.forward * v;
            _dashDirection = inputDir == Vector3.zero ? transform.forward : inputDir.normalized;

            _isDashing = true;
            _dashTimer = dashDuration;
            _dashCooldownTimer = dashCooldown;
        }
    }

    void HandleDash()
    {
        _dashTimer -= Time.deltaTime;

        _cc.Move(_dashDirection * dashSpeed * Time.deltaTime);

        if (_dashTimer <= 0f)
            _isDashing = false;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}