using UnityEngine;

public class CameraController2D : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;


    [Header("Movement")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset;

    [Header("Zoom")]
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float zoomLimiter = 10f;

    [Header("Bounds")]
	[SerializeField] private Vector2 minBounds;
	[SerializeField] private Vector2 maxBounds;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (player1 == null || player2 == null) return;

        MoveCamera();
        ZoomCamera();
    }

    private void MoveCamera()
{
    Vector3 centerPoint = GetCenterPoint();
    Vector3 newPosition = centerPoint + offset;

    float camHeight = cam.orthographicSize;
    float camWidth = camHeight * cam.aspect;

    float clampedX = Mathf.Clamp(newPosition.x, minBounds.x + camWidth, maxBounds.x - camWidth);
    float clampedY = Mathf.Clamp(newPosition.y, minBounds.y + camHeight, maxBounds.y - camHeight);

    Vector3 clampedPosition = new Vector3(clampedX, clampedY, newPosition.z);

    transform.position = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed * Time.deltaTime);
}

    private void ZoomCamera()
    {
        float distance = Vector2.Distance(player1.position, player2.position);

        float newZoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    private Vector3 GetCenterPoint()
    {
        return (player1.position + player2.position) / 2f;
    }
}