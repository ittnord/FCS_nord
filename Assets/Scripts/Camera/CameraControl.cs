using Character;
using FCS.Managers;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    public static CameraControl sInstance;

    public float DampTime = 0.2f;                   // Approximate time for the camera to refocus.
    public float ScreenEdgeBuffer = 4f;             // Space between the top/bottom most target and the screen edge (multiplied by aspect for left and right).
    public float MinSize = 6.5f;                    // The smallest orthographic size the camera can be.

    private Camera _camera;                        // Used for referencing the camera.
    private float _zoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
    private Vector3 _moveVelocity;                 // Reference velocity for the smooth damping of the position.
    private float _convertDistanceToSize;                 // Used to multiply by the offset of the rig to the furthest target.


    private void Awake()
    {
        sInstance = this;
        _camera = GetComponentInChildren<Camera>();
    }


    private void Start()
    {
        SetDistanceToSize();
    }


    private void SetDistanceToSize()
    {
        // If the camera's orthographic size will be affected by how much it is tilted more than by it's aspect ratio...
        if (VisibleAreaWiderThanTall())
            // ... then the ratio of distance to orthographic size is based on the camera's tilt.
            _convertDistanceToSize = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x);
        else
            // Otherwise the ratio of distance to orthographic size is simply the aspect ratio.
            _convertDistanceToSize = 1f / _camera.aspect;
    }


    private bool VisibleAreaWiderThanTall()
    {
        // The project frustum is how the camera's frustum would appear on a flat horizontal plane.
        // This variable is a measure of the projected frustum's height vs it's width.
        float projectedFrustumRatio = Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x) * _camera.aspect;

        // When the ratio is greater than 1 the projected frustum is taller than it is wide.
        // This means the tilt of the camera is more important than it's aspect in calculating the orthographic size.
        return projectedFrustumRatio > 1f;
    }


    private void FixedUpdate()
    {
        // The camera is moved towards a target position which is returned.
        Vector3 targetPosition = Move();

        // The size is changed based on where the camera is going to be.
        Zoom(targetPosition);
    }


    private Vector3 Move()
    {
        // Find the average position of the targets and smoothly transition to that position.
        Vector3 targetPosition = FindAveragePosition();
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _moveVelocity, DampTime);

        return targetPosition;
    }


    private Vector3 FindAveragePosition()
    {
        Vector3 average = new Vector3();
        int numTargets = 0;
            
        // Go through all the targets and add their positions together.
        for (int i = 0; i < GameManager.Characters.Count; i++)
        {
            // If the target isn't active, go on to the next one.
            if (!GameManager.Characters[i].Instance.activeSelf)
                continue;

            if(!GameManager.Characters[i].Instance.GetComponent<CharacterMovement>().isLocalPlayer)
                continue;
            // Add to the average and increment the number of targets in the average.
            average += GameManager.Characters[i].Instance.transform.position;
            numTargets++;
        }

        // If there are targets divide the sum of the positions by the number of them to find the average.
        if (numTargets > 0)
            average /= numTargets;

        // Keep the same y value.
        average.y = transform.position.y;

        return average;
    }


    private void Zoom(Vector3 desiredPosition)
    {
        // Find the required size based on the desired position
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
        // Default furthest distance is no distance at all.
        float furthestDistance = 0f;

        // Go through all the targets and if they are further away use that distance instead.
        for (int i = 0; i < GameManager.Characters.Count; i++)
        {
            // If the target isn't active, on to the next one.
            if (!GameManager.Characters[i].Instance.activeSelf)
                continue;

            if(!GameManager.Characters[i].Instance.GetComponent<CharacterMovement>().isLocalPlayer)
                continue;
            
            // Find the distance from the camera's desired position to the target.
            float targetDistance = (desiredPosition - GameManager.Characters[i].Instance.transform.position).magnitude;

            // If it's greater than the current furthest distance, it's the furthest distance.
            if (targetDistance > furthestDistance)
            {
                furthestDistance = targetDistance;
            }
        }

        // Return the distance to the target that is furthest away.
        return furthestDistance;
    }


    public void SetAppropriatePositionAndSize()
    {
        // Set orthographic size and position without damping.
        transform.position = FindAveragePosition();
        _camera.orthographicSize = FindRequiredSize(transform.position);
    }
}