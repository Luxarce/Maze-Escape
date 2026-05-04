/*
using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public int doorID = 1;
    public float slideDistance = 3f;
    public float slideSpeed = 4f;

    [Header("Slide Direction")]
    public Vector3 slideDirection = Vector3.right;

    [Header("UI")]
    public TMP_Text hintText;

    private bool _isOpen = false;
    private bool _isSliding = false;
    private bool _isClosing = false;
    private bool _playerNearby = false;
    private Vector3 _closedPosition;
    private Vector3 _openPosition;

    void Start()
    {
        _closedPosition = transform.position;
        _openPosition = _closedPosition + slideDirection.normalized * slideDistance;

        if (hintText != null)
            hintText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Slide open
        if (_isSliding)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _openPosition,
                slideSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, _openPosition) < 0.01f)
            {
                transform.position = _openPosition;
                _isSliding = false;
                _isOpen = true;
            }
        }

        // Slide close
        if (_isClosing)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _closedPosition,
                slideSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, _closedPosition) < 0.01f)
            {
                transform.position = _closedPosition;
                _isClosing = false;
                _isOpen = false;
            }
        }

        // Interaction
        if (_playerNearby && !_isOpen && !_isSliding && !_isClosing)
        {
            if (PlayerKeyInventory.Instance.HasKey(doorID))
            {
                if (hintText != null)
                {
                    hintText.gameObject.SetActive(true);
                    hintText.text = "[ Press Q to open Door " + doorID + " ]";
                }

                if (Input.GetKeyDown(KeyCode.Q))
                    StartSlide();
            }
            else
            {
                if (hintText != null)
                {
                    hintText.gameObject.SetActive(true);
                    hintText.text = "Collect Key " + doorID + " to unlock this door!";
                }
            }
        }
    }

    void StartSlide()
    {
        _isSliding = true;
        _isClosing = false;
        PlayerKeyInventory.Instance.UseKey(doorID);

        if (hintText != null)
            hintText.gameObject.SetActive(false);
    }

    public void CloseDoor()
    {
        if (_isOpen)
        {
            _isOpen = false;
            _isClosing = true;
            _isSliding = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            _playerNearby = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNearby = false;

            if (hintText != null)
                hintText.gameObject.SetActive(false);
        }
    }
}

*/

using UnityEngine;
using TMPro;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public int doorID = 1;
    public float slideDistance = 3f;
    public float slideSpeed = 4f;

    [Header("Slide Direction")]
    public Vector3 slideDirection = Vector3.right;

    [Header("UI")]
    public TMP_Text hintText;

    private bool _isOpen = false;
    private bool _isSliding = false;
    private bool _isClosing = false;
    private bool _playerNearby = false;
    private bool _permanentlyOpen = false;
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
                _isOpen = true;
            }
        }

        if (_isClosing)
        {
            transform.position = Vector3.MoveTowards(transform.position, _closedPosition, slideSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, _closedPosition) < 0.01f)
            {
                transform.position = _closedPosition;
                _isClosing = false;
                _isOpen = false;
            }
        }

        if (_playerNearby && !_isSliding && !_isClosing && !_permanentlyOpen)
        {
            if (PlayerKeyInventory.Instance.HasKey(doorID))
            {
                if (!_isOpen)
                {
                    if (hintText != null)
                    {
                        hintText.gameObject.SetActive(true);
                        hintText.text = "[ Press Q to open Door " + doorID + " ]";
                    }
                    if (Input.GetKeyDown(KeyCode.Q))
                        StartSlide();
                }
                else
                {
                    if (hintText != null) hintText.gameObject.SetActive(false);
                }
            }
            else
            {
                if (hintText != null)
                {
                    hintText.gameObject.SetActive(true);
                    hintText.text = "You need the key to open this door!";
                }
            }
        }
    }

    void StartSlide()
    {
        _isSliding = true;
        _isClosing = false;
        PlayerKeyInventory.Instance.UseKey(doorID);
        if (hintText != null) hintText.gameObject.SetActive(false);
    }

    public void OpenPermanently()
    {
        _permanentlyOpen = true;
        _isSliding = true;
        _isClosing = false;
        _isOpen = false;
        if (hintText != null) hintText.gameObject.SetActive(false);
        Debug.Log("Door " + doorID + " permanently opened!");
    }

    public void CloseDoor()
    {
        if (_permanentlyOpen) return;
        if (_isOpen)
        {
            _isOpen = false;
            _isClosing = true;
            _isSliding = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNearby = true;
            if (PlayerKeyInventory.Instance.holdDisplay != null)
                PlayerKeyInventory.Instance.holdDisplay.ShowKeyIfOwned(doorID);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerNearby = false;
            if (hintText != null) hintText.gameObject.SetActive(false);
            if (PlayerKeyInventory.Instance.holdDisplay != null)
                PlayerKeyInventory.Instance.holdDisplay.HideKey(doorID);
        }
    }
}
