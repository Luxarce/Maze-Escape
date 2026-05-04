using UnityEngine;
using TMPro;

public class GlassShard : MonoBehaviour
{
    public float floatHeight = 0.3f;
    public float floatSpeed = 2f;
    public float rotateSpeed = 90f;
    public TMP_Text hintText;

    private Vector3 startPos;
    private bool nearPlayer = false;
    private bool collected = false;

    void Start()
    {
        startPos = transform.position;
        if (hintText != null)
            hintText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (collected) return;

        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        if (nearPlayer && Input.GetKeyDown(KeyCode.E))
        {
            collected = true;
            nearPlayer = false;

            if (CandleTableManager.Instance != null)
                CandleTableManager.Instance.CollectShard(gameObject);

            if (ShardHoldDisplay.Instance != null)
            {
                ShardHoldDisplay.Instance.PickedUpShard();
                Debug.Log("PickedUpShard called successfully!");
            }
            else
            {
                Debug.LogError("ShardHoldDisplay Instance is NULL!");
            }

            if (hintText != null)
                hintText.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (other.CompareTag("Player"))
        {
            nearPlayer = true;
            if (hintText != null)
            {
                hintText.gameObject.SetActive(true);
                hintText.text = "Press E to pick up glass shard";
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            nearPlayer = false;
            if (hintText != null)
                hintText.gameObject.SetActive(false);
        }
    }
}