using UnityEngine;

public class RotateVisual : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 180f;

    private Transform targetToTrack;
    private Vector3 lastPosition;

    void Start()
    {
        targetToTrack = transform.parent != null ? transform.parent : transform;
        lastPosition = targetToTrack.position;
    }

    void Update()
    {
        float moveX = targetToTrack.position.x - lastPosition.x;

        float direction = 0f;

        if (moveX > 0.01f)
        {
            direction = -1f; // 
        }
        else if (moveX < -0.01f)
        {
            direction = 1f; //
        }

        transform.Rotate(0f, 0f, rotationSpeed * direction * Time.deltaTime);

        lastPosition = targetToTrack.position;
    }
}