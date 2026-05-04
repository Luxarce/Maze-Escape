using UnityEngine;
using TMPro;

public class CandleSpot : MonoBehaviour
{
    [Header("Spot Settings")]
    public bool isMiddleSpot = false;

    [Header("Pre placed shard with light inside - disable by default")]
    public GameObject preGlassShard;

    [Header("Float after placed")]
    public float floatHeight = 0.2f;
    public float floatSpeed = 1.5f;

    [Header("Middle spot glow light - only on middle spot")]
    public Light middleGlowLight;

    [Header("UI")]
    public TMP_Text hintText;

    private bool _playerNearby = false;
    private bool _itemPlaced = false;
    private Vector3 _floatOrigin;

    void Start()
    {
        if (hintText != null) hintText.gameObject.SetActive(false);
        if (middleGlowLight != null) middleGlowLight.enabled = false;
        if (preGlassShard != null) preGlassShard.SetActive(false);
    }

    void Update()
    {
        if (_itemPlaced && preGlassShard != null)
        {
            float newY = _floatOrigin.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            preGlassShard.transform.position = new Vector3(
                _floatOrigin.x, newY, _floatOrigin.z);
        }

        if (_itemPlaced) return;

        if (_playerNearby)
        {
            if (isMiddleSpot && CandleTableManager.Instance.SidePlacedCount < 4)
            {
                if (hintText != null)
                {
                    hintText.gameObject.SetActive(true);
                    hintText.text = "Place the other shards first!";
                }
                return;
            }

            if (CandleTableManager.Instance.PlayerHasShard())
            {
                if (hintText != null)
                {
                    hintText.gameObject.SetActive(true);
                    hintText.text = "[ Press E to place glass shard ]";
                }
                if (Input.GetKeyDown(KeyCode.E))
                    PlaceItem();
            }
            else
            {
                if (hintText != null)
                {
                    hintText.gameObject.SetActive(true);
                    hintText.text = "Find a glass shard first!";
                }
            }
        }
    }

    void PlaceItem()
    {
        _itemPlaced = true;
        CandleTableManager.Instance.OnShardPlaced(isMiddleSpot);

        // Hide shard model on screen
        if (ShardHoldDisplay.Instance != null)
            ShardHoldDisplay.Instance.PlacedShard();

        // Show pre placed shard on candle holder
        if (preGlassShard != null)
        {
            preGlassShard.SetActive(true);
            _floatOrigin = preGlassShard.transform.position;
        }

        if (hintText != null) hintText.gameObject.SetActive(false);
        Debug.Log("Shard placed on " + (isMiddleSpot ? "MIDDLE" : "SIDE") + " spot!");
    }

    public void ActivateMiddleGlow()
    {
        if (middleGlowLight != null)
            middleGlowLight.enabled = true;
        Debug.Log("Middle spot glowing!");
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
