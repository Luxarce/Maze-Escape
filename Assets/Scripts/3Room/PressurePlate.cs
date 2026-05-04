using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Plate Settings")]
    public int plateID = 1;

    [Header("Connected Door")]
    public GameObject connectedDoor;
    public float slideDistance = 3f;
    public float slideSpeed = 3f;
    public Vector3 slideDirection = new Vector3(0, -1, 0);

    [Header("Visual - change color when activated")]
    public Renderer plateRenderer;
    public Color inactiveColor = Color.red;
    public Color activeColor = Color.green;

    private bool _isActivated = false;
    private int _objectsOnPlate = 0;
    private Vector3 _doorClosedPos;
    private Vector3 _doorOpenPos;
    private bool _isSliding = false;
    private bool _isOpening = false;

    void Start()
    {
        if (plateRenderer != null)
            plateRenderer.material.color = inactiveColor;

        if (connectedDoor != null)
        {
            _doorClosedPos = connectedDoor.transform.position;
            _doorOpenPos = _doorClosedPos + slideDirection.normalized * slideDistance;
        }
    }

    void Update()
    {
        if (connectedDoor == null || !_isSliding) return;

        Vector3 target = _isOpening ? _doorOpenPos : _doorClosedPos;
        connectedDoor.transform.position = Vector3.MoveTowards(
            connectedDoor.transform.position, target, slideSpeed * Time.deltaTime);

        if (Vector3.Distance(connectedDoor.transform.position, target) < 0.01f)
        {
            connectedDoor.transform.position = target;
            _isSliding = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable") || other.CompareTag("Player"))
        {
            _objectsOnPlate++;
            if (!_isActivated)
            {
                _isActivated = true;
                if (plateRenderer != null)
                    plateRenderer.material.color = activeColor;

                // Open door
                _isOpening = true;
                _isSliding = true;

                // Also notify platform manager if used for that
                if (Room3PlatformManager.Instance != null)
                    Room3PlatformManager.Instance.PlateActivated(plateID);

                Debug.Log("Plate " + plateID + " activated! Door opening!");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pushable") || other.CompareTag("Player"))
        {
            _objectsOnPlate--;
            if (_objectsOnPlate <= 0 && _isActivated)
            {
                _objectsOnPlate = 0;
                _isActivated = false;
                if (plateRenderer != null)
                    plateRenderer.material.color = inactiveColor;

                // Close door
                _isOpening = false;
                _isSliding = true;

                if (Room3PlatformManager.Instance != null)
                    Room3PlatformManager.Instance.PlateDeactivated(plateID);

                Debug.Log("Plate " + plateID + " deactivated! Door closing!");
            }
        }
    }
}

