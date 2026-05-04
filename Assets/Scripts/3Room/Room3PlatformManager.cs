using UnityEngine;
using System.Collections.Generic;

public class Room3PlatformManager : MonoBehaviour
{
    public static Room3PlatformManager Instance;

    [Header("Settings")]
    public int totalPlatesNeeded = 3;

    [Header("Platform to lower")]
    public FloatingPlatform platform;

    private HashSet<int> _activatedPlates = new HashSet<int>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlateActivated(int plateID)
    {
        _activatedPlates.Add(plateID);
        Debug.Log("Plates activated: " + _activatedPlates.Count + " / " + totalPlatesNeeded);

        if (_activatedPlates.Count >= totalPlatesNeeded)
            OnAllPlatesActivated();
    }

    public void PlateDeactivated(int plateID)
    {
        _activatedPlates.Remove(plateID);
        Debug.Log("Plate deactivated! Active plates: " + _activatedPlates.Count);

        // Raise platform back up if not all plates active
        if (platform != null)
            platform.RaisePlatform();
    }

    void OnAllPlatesActivated()
    {
        Debug.Log("All plates activated! Lowering platform!");
        if (platform != null)
            platform.LowerPlatform();
    }
}
