
using UnityEngine;
using TMPro;

public class KeyPickupPlatform : MonoBehaviour
{
    [Header("Key Settings")]
    public int keyID = 1;

    [Header("Rotate Settings")]
    public float rotateSpeed = 90f;

    [Header("UI")]
    public TMP_Text hintText;

    private bool _playerNearby = false;
    private bool _collected = false;

    void Start()
    {
        if (hintText != null)
            hintText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (_collected) return;

        // Only rotates, no floating - platform controls the position!
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



