using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private bool destroyOnHit = true;

    private Vector2 moveDirection = Vector2.right;
    private bool initialized = false;

    public void Initialize(Vector2 direction)
    {
        moveDirection = direction.normalized;
        initialized = true;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (!initialized) return;

        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
            return;

        LinkEnemyTarget enemy = other.GetComponent<LinkEnemyTarget>();
        if (enemy != null)
        {
            enemy.DieFromLink();

            if (destroyOnHit)
            {
                Destroy(gameObject);
                return;
            }
        }

        if (!other.isTrigger)
        {
            if (destroyOnHit)
                Destroy(gameObject);
        }
    }
}