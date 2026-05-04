using UnityEngine;

/// <summary>
/// CameraRotation.cs
/// Attach to the Camera (child of Player).
/// </summary>
public class CameraRotation : MonoBehaviour
{
    [Header("Look Settings")]
    public float mouseSensitivity = 200f;
    public float verticalClamp = 80f;

    [Header("References")]
    public Transform playerBody;

    private float _xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleCursorToggle();
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -verticalClamp, verticalClamp);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    void HandleCursorToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0) && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}