
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 moveDirection = new Vector3(1, 0, 0);
    public float moveDistance = 3f;
    public float moveSpeed = 2f;

    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private bool _movingForward = true;

    void Start()
    {
        _startPosition = transform.position;
        _targetPosition = _startPosition + moveDirection.normalized * moveDistance;
    }

    void Update()
    {
        if (_movingForward)
        {
            transform.position = Vector3.MoveTowards(
                transform.position, _targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
                _movingForward = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position, _startPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _startPosition) < 0.01f)
                _movingForward = true;
        }
    }
}
