using Character;
using FCS.Managers;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    public static CameraControl Instance;

    public float DampTime = 0.2f;                   // Approximate time for the camera to refocus.
    public float ScreenEdgeBuffer = 4f;             // Space between the top/bottom most target and the screen edge (multiplied by aspect for left and right).
    public float MinSize = 6.5f;                    // The smallest orthographic size the camera can be.

    private Camera _camera;                        // Used for referencing the camera.
    private float _zoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 _moveVelocity;                 // Reference velocity for the smooth damping of the position.
    private float _convertDistanceToSize;                 // Used to multiply by the offset of the rig to the furthest target.

    private CharacterMovement _targetCharacter;
    
    private void Awake()
    {
        Instance = this;
        _camera = GetComponentInChildren<Camera>();
    }

    public void SetCharacter(CharacterMovement characterMovement)
    {
        _targetCharacter = characterMovement;
    }

    private void Start()
    {
        SetDistanceToSize();
    }

    private void SetDistanceToSize()
    {
        if (VisibleAreaWiderThanTall())
            _convertDistanceToSize = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x);
        else
            _convertDistanceToSize = 1f / _camera.aspect;
    }

    private bool VisibleAreaWiderThanTall()
    {
         float projectedFrustumRatio = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x) * _camera.aspect;
    return projectedFrustumRatio > 1f;
    }


    private void FixedUpdate()
    {
        Vector3 targetPosition = Move();
        Zoom(targetPosition);
    }


    private Vector3 Move()
    {
        Vector3 targetPosition = FindAveragePosition();
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _moveVelocity, DampTime);
        return targetPosition;
    }


    private Vector3 FindAveragePosition()
    {
        var position = _targetCharacter != null ? _targetCharacter.transform.position : Vector3.zero;
        position.y = transform.position.y;
        return position;
    }


    private void Zoom(Vector3 desiredPosition)
    {
        float targetSize = FindRequiredSize(desiredPosition);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, targetSize, ref _zoomSpeed,
            DampTime);
    }


    private float FindRequiredSize(Vector3 desiredPosition)
    {
        // Find how far from the rig to the furthest target.
        float targetDistance = MaxTargetDistance(desiredPosition);

        // Calculate the size based on the previously found ratio and buffer.
        float newSize = targetDistance * _convertDistanceToSize + ScreenEdgeBuffer;

        // Restrict the new size so that it's not smaller than the minimum size.
        newSize = Mathf.Max(newSize, MinSize);

        return newSize;
    }


    private float MaxTargetDistance(Vector3 desiredPosition)
    {
        float furthestDistance = 0f;

        if (_targetCharacter != null)
        {
            var targetDistance = (desiredPosition - (_targetCharacter != null ? _targetCharacter.transform.position : Vector3.zero)).magnitude;
            if (targetDistance > furthestDistance)
            {
                furthestDistance = targetDistance;
            }
        }
        return furthestDistance;
    }

    public void SetAppropriatePositionAndSize()
    {
        // Set orthographic size and position without damping.
        transform.position = FindAveragePosition();
        _camera.orthographicSize = FindRequiredSize(transform.position);
    }
}
