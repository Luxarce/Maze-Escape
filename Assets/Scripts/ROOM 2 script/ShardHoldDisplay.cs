
using UnityEngine;
using TMPro;

public class ShardHoldDisplay : MonoBehaviour
{
    public static ShardHoldDisplay Instance;

    [Header("3D Shard Model")]
    public GameObject shardModel;

    [Header("UI Counter")]
    public TMP_Text shardCounterText;

    private int _shardsCarrying = 0;
    private int _totalShards = 5;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (shardModel != null) shardModel.SetActive(false);
        UpdateDisplay();
        Debug.Log("ShardHoldDisplay START - counter text is: " + (shardCounterText != null ? "assigned" : "NULL"));
    }

    public void PickedUpShard()
    {
        _shardsCarrying++;
        Debug.Log("PickedUpShard called! Carrying: " + _shardsCarrying);
        if (shardModel != null) shardModel.SetActive(true);
        UpdateDisplay();
    }

    public void PlacedShard()
    {
        _shardsCarrying--;
        if (_shardsCarrying <= 0)
        {
            _shardsCarrying = 0;
            if (shardModel != null) shardModel.SetActive(false);
        }
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (shardCounterText != null)
        {
            shardCounterText.text = "Shards: " + _shardsCarrying + " / " + _totalShards;
            Debug.Log("Counter updated to: " + shardCounterText.text);
        }
        else
        {
            Debug.LogError("ShardCounterText is NULL! Drag it into the slot!");
        }
    }
}


