using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    public float lowerSpeed = 2f;
    public float raiseSpeed = 2f;
    public float lowerAmount = 5f;

    private Vector3 _startPosition;
    private Vector3 _loweredPosition;
    private bool _isLowering = false;
    private bool _isRaising = false;
    private bool _isLowered = false;

    void Start()
    {
        _startPosition = transform.position;
        _loweredPosition = _startPosition - new Vector3(0, lowerAmount, 0);
    }

    void Update()
    {
        if (_isLowering)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, _loweredPosition, lowerSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _loweredPosition) < 0.01f)
            {
                transform.position = _loweredPosition;
                _isLowering = false;
                _isLowered = true;
                Debug.Log("Platform fully lowered!");
            }
        }

        if (_isRaising)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, _startPosition, raiseSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _startPosition) < 0.01f)
            {
                transform.position = _startPosition;
                _isRaising = false;
                _isLowered = false;
                Debug.Log("Platform raised back up!");
            }
        }
    }

    public void LowerPlatform()
    {
        if (!_isLowered)
        {
            _isLowering = true;
            _isRaising = false;
        }
    }

    public void RaisePlatform()
    {
        if (_isLowered || _isLowering)
        {
            _isRaising = true;
            _isLowering = false;
        }
    }
}
