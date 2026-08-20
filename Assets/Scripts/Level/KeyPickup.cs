using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DoorController targetDoor;

    [Header("Flight Settings")]
    [SerializeField] private float flySpeed = 8f;
    [SerializeField] private float stopDistance = 0.3f;
    [SerializeField] private Vector3 doorOffset = new Vector3(0f, 1f, 0f);

    [Header("Visual")]
    [SerializeField] private bool hideAfterDelivery = true;

    private bool isCollected = false;
    private bool isFlyingToDoor = false;
    private bool delivered = false;

    private Collider2D keyCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        keyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isFlyingToDoor && targetDoor != null)
        {
            FlyToDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            CollectKey();
        }
    }

    private void CollectKey()
    {
        isCollected = true;
        isFlyingToDoor = true;

        if (keyCollider != null)
            keyCollider.enabled = false;
    }

    private void FlyToDoor()
    {
        Vector3 targetPosition = targetDoor.transform.position + doorOffset;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            flySpeed * Time.deltaTime
        );

        transform.Rotate(0f, 0f, 360f * Time.deltaTime);

        if (!delivered && Vector3.Distance(transform.position, targetPosition) <= stopDistance)
        {
            delivered = true;
            isFlyingToDoor = false;

            targetDoor.RegisterCollectedKey();

            if (hideAfterDelivery)
            {
                if (spriteRenderer != null)
                    spriteRenderer.enabled = false;

                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}