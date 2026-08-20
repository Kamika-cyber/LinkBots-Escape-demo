using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement")]
    [SerializeField] private float speed = 2f;

    [Header("Settings")]
    [SerializeField] private float reachDistance = 0.1f;

    private Transform currentTarget;

    private void Start()
    {
        currentTarget = pointB;
    }

    private void Update()
    {
        if (currentTarget == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            currentTarget.position,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, currentTarget.position) < reachDistance)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1") ||
            collision.gameObject.CompareTag("Player2"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeHit();
            }
            else
            {
                if (GameOverUI.Instance != null)
                    GameOverUI.Instance.ShowGameOver();
                else
                    SceneLoader.ReloadCurrentLevel();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(pointA.position, pointB.position);
        Gizmos.DrawSphere(pointA.position, 0.15f);
        Gizmos.DrawSphere(pointB.position, 0.15f);
    }
}