using UnityEngine;
using TMPro;

public class PushableObject : MonoBehaviour
{
    [Header("Push Settings")]
    public float pushSpeed = 3f;

    [Header("UI")]
    public TMP_Text hintText;

    private bool _playerNearby = false;
    private bool _isBeingPushed = false;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        if (hintText != null) hintText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_playerNearby)
        {
            // Start pushing when E is held
            if (Input.GetKey(KeyCode.E))
            {
                _isBeingPushed = true;
                if (hintText != null)
                {
                    hintText.gameObject.SetActive(true);
                    hintText.text = "[ Hold E to push ]";
                }
            }
            else
            {
                _isBeingPushed = false;
                if (hintText != null)
                {
                    hintText.gameObject.SetActive(true);
                    hintText.text = "[ Hold E to push ]";
                }
            }
        }

        if (_isBeingPushed)
        {
            Vector3 moveDir = new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")
            );

            moveDir = Camera.main.transform.TransformDirection(moveDir);
            moveDir.y = 0;
            moveDir.Normalize();

            _rb.linearVelocity = moveDir * pushSpeed;
        }
        else
        {
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNearby = true;
            if (hintText != null)
            {
                hintText.gameObject.SetActive(true);
                hintText.text = "[ Hold E to push ]";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNearby = false;
            _isBeingPushed = false;
            _rb.linearVelocity = Vector3.zero;
            if (hintText != null) hintText.gameObject.SetActive(false);
        }
    }
}
