using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private bool destroyOnHit = true;
    [SerializeField] private bool rotateToVelocity = true;

    private Rigidbody2D rb;
    private bool initialized = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 velocity)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = velocity;
        initialized = true;

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (!initialized || !rotateToVelocity || rb == null) return;

        Vector2 velocity = rb.linearVelocity;

        if (velocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
                player.TakeHit();
            else
                SceneLoader.ReloadCurrentLevel();

            if (destroyOnHit)
                Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            if (destroyOnHit)
                Destroy(gameObject);
        }
    }
}