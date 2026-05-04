


/*using UnityEngine;
using TMPro;

public class KeyPickup : MonoBehaviour
{
    [Header("Key Settings")]
    public int keyID = 1;

    [Header("Float Settings")]
    public float floatHeight = 0.3f;
    public float floatSpeed = 2f;
    public float rotateSpeed = 90f;

    [Header("UI")]
    public TMP_Text hintText;

    private Vector3 _startPosition;
    private bool _playerNearby = false;
    private bool _collected = false;

    void Start()
    {
        _startPosition = transform.position;

        if (hintText != null)
            hintText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_collected) return;

        // Float + spin
        float newY = _startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        if (_playerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E))
                Collect();
        }
    }

    void Collect()
    {
        _collected = true;
        _playerNearby = false;

        PlayerKeyInventory.Instance.CollectKey(keyID);

        if (hintText != null)
            hintText.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_collected) return;

        if (other.CompareTag("Player"))
        {
            _playerNearby = true;

            if (hintText != null)
            {
                hintText.gameObject.SetActive(true);
                hintText.text = "[ Press E to collect Key " + keyID + " ]";
            }
        }
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

public class KeyPickup : MonoBehaviour
{
    [Header("Key Settings")]
    public int keyID = 1;

    [Header("Float Settings")]
    public float floatHeight = 0.3f;
    public float floatSpeed = 2f;
    public float rotateSpeed = 90f;

    [Header("Disable float if on moving platform")]
    public bool disableFloat = false;

    [Header("UI")]
    public TMP_Text hintText;

    private Vector3 _startPosition;
    private bool _playerNearby = false;
    private bool _collected = false;

    void Start()
    {
        _startPosition = transform.position;
        if (hintText != null)
            hintText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_collected) return;

        // Only float if not on a moving platform
        if (!disableFloat)
        {
            float newY = _startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector3(_startPosition.x, newY, _startPosition.z);
        }

        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        if (_playerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E))
                Collect();
        }
    }

    void Collect()
    {
        _collected = true;
        _playerNearby = false;
        PlayerKeyInventory.Instance.CollectKey(keyID);
        if (hintText != null)
            hintText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (_collected) return;
        if (other.CompareTag("Player"))
        {
            _playerNearby = true;
            if (hintText != null)
            {
                hintText.gameObject.SetActive(true);
                hintText.text = "[ Press E to collect Key " + keyID + " ]";
            }
        }
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



