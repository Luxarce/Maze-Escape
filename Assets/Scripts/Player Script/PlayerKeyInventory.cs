/*using UnityEngine;
using System.Collections.Generic;

public class PlayerKeyInventory : MonoBehaviour
{
    public static PlayerKeyInventory Instance;

    public KeyHoldDisplay holdDisplay;

    private HashSet<int> _collectedKeys = new HashSet<int>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CollectKey(int keyID)
    {
        if (_collectedKeys.Contains(keyID)) return;

        _collectedKeys.Add(keyID);
        Debug.Log("Collected Key " + keyID);

        if (holdDisplay != null)
            holdDisplay.ShowKey(keyID);
        else
            Debug.LogError("Hold Display is empty! Drag ItemHolder into GameManager.");
    }

    public bool HasKey(int keyID)
    {
        return _collectedKeys.Contains(keyID);
    }

    public void UseKey(int keyID)
    {
        _collectedKeys.Remove(keyID);

        if (holdDisplay != null)
            holdDisplay.HideKey(keyID);
    }

    public int GetKeyCount()
    {
        return _collectedKeys.Count;
    }
}

*/

using UnityEngine;
using System.Collections.Generic;

public class PlayerKeyInventory : MonoBehaviour
{
    public static PlayerKeyInventory Instance;
    public KeyHoldDisplay holdDisplay;

    private HashSet<int> _collectedKeys = new HashSet<int>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CollectKey(int keyID)
    {
        if (_collectedKeys.Contains(keyID)) return;
        _collectedKeys.Add(keyID);
        Debug.Log("Collected Key " + keyID);

        // Update key counter UI
        if (KeyCounterUI.Instance != null)
            KeyCounterUI.Instance.AddKey();

        if (holdDisplay != null)
            holdDisplay.ShowKey(keyID);
        else
            Debug.LogError("Hold Display is empty!");
    }

    public bool HasKey(int keyID)
    {
        return _collectedKeys.Contains(keyID);
    }

    public void UseKey(int keyID)
    {
        // Key is kept permanently
        Debug.Log("Used Key " + keyID + " (kept in inventory)");
    }

    public int GetKeyCount()
    {
        return _collectedKeys.Count;
    }
}
