using UnityEngine;

public class CandleTableManager : MonoBehaviour
{
    public static CandleTableManager Instance;

    [Header("Middle spot reference")]
    public CandleSpot middleSpot;

    [Header("Password Point Light - disable by default")]
    public Light passwordLight;

    private int _shardsInHand = 0;
    private int _sidePlacedCount = 0;

    public int SidePlacedCount { get { return _sidePlacedCount; } }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (passwordLight != null) passwordLight.enabled = false;
    }

    // Called by GlassShard when player picks one up
    public void CollectShard(GameObject shardObj)
    {
        _shardsInHand++;
        Debug.Log("Shards in hand: " + _shardsInHand);
    }

    public bool PlayerHasShard()
    {
        return _shardsInHand > 0;
    }

    // Called by CandleSpot when shard is placed
    public void OnShardPlaced(bool isMiddle)
    {
        _shardsInHand--;

        if (isMiddle)
        {
            // All done! Turn on password light
            OnAllPlaced();
        }
        else
        {
            _sidePlacedCount++;
            Debug.Log("Side spots filled: " + _sidePlacedCount + " / 4");

            // All 4 sides done - activate middle glow
            if (_sidePlacedCount >= 4)
            {
                if (middleSpot != null)
                    middleSpot.ActivateMiddleGlow();
                Debug.Log("4 sides placed! Middle spot is now active!");
            }
        }
    }

    void OnAllPlaced()
    {
        Debug.Log("All 5 shards placed! Password light activating!");
        if (passwordLight != null)
            passwordLight.enabled = true;
    }
}
