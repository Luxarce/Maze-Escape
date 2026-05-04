using UnityEngine;
using TMPro;

public class ActivationDoor : MonoBehaviour
{
    public float slideDistance = 3f;
    public float slideSpeed = 4f;
    public Vector3 slideDirection = Vector3.right;
    public TMP_Text hintText;
    public KeypadUI keypad;

    private bool _unlocked = false;
    private bool _isSliding = false;
    private bool _playerNearby = false;
    private Vector3 _closedPosition;
    private Vector3 _openPosition;

    void Start()
    {
        _closedPosition = transform.position;
        _openPosition = _closedPosition + slideDirection.normalized * slideDistance;
        if (hintText != null) hintText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_isSliding)
        {
            transform.position = Vector3.MoveTowards(transform.position, _openPosition, slideSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _openPosition) < 0.01f)
            {
                transform.position = _openPosition;
                _isSliding = false;
            }
        }

        if (_playerNearby && !_unlocked && !_isSliding)
        {
            if (hintText != null)
            {
                hintText.gameObject.SetActive(true);
                hintText.text = "[ Press E to enter password ]";
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (keypad != null) keypad.OpenKeypad();
            }
        }
    }

    public void Unlock()
    {
        _unlocked = true;
        _isSliding = true;
        if (hintText != null) hintText.gameObject.SetActive(false);
        Debug.Log("Activation door unlocked!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _playerNearby = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNearby = false;
            if (hintText != null) hintText.gameObject.SetActive(false);
        }
    }
}
