using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform turretVisual;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 8f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private bool requireLineOfSight = false;

    [Header("Shoot Settings")]
    [SerializeField] private float shootInterval = 2f;
    [SerializeField] private bool shootOnStart = false;

    [Header("Projectile")]
    [SerializeField] private float projectileSpeed = 6f;
    [SerializeField] private bool useArcShot = false;
    [SerializeField] private float upwardForce = 3f;

    [Header("Aiming")]
    [SerializeField] private bool rotateTurretVisual = true;
    [SerializeField] private float rotationOffset = 0f;

    private Transform currentTarget;
    private float timer;

    private void Start()
    {
        timer = shootInterval;

        if (shootOnStart)
        {
            FindNearestTarget();

            if (currentTarget != null)
                Shoot();
        }
    }

    private void Update()
    {
        FindNearestTarget();
        AimAtTarget();

        if (currentTarget == null)
            return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            Shoot();
            timer = shootInterval;
        }
    }

    private void FindNearestTarget()
    {
        GameObject p1 = GameObject.FindGameObjectWithTag("Player1");
        GameObject p2 = GameObject.FindGameObjectWithTag("Player2");

        Transform bestTarget = null;
        float bestDistance = Mathf.Infinity;

        TryCheckTarget(p1, ref bestTarget, ref bestDistance);
        TryCheckTarget(p2, ref bestTarget, ref bestDistance);

        currentTarget = bestTarget;
    }

    private void TryCheckTarget(GameObject candidate, ref Transform bestTarget, ref float bestDistance)
    {
        if (candidate == null) return;

        Transform targetTransform = candidate.transform;
        float distance = Vector2.Distance(transform.position, targetTransform.position);

        if (distance > detectionRadius)
            return;

        if (requireLineOfSight)
        {
            Vector2 start = firePoint != null ? firePoint.position : transform.position;
            Vector2 end = targetTransform.position;

            RaycastHit2D hit = Physics2D.Linecast(start, end, obstacleLayer);
            if (hit.collider != null)
                return;
        }

        if (distance < bestDistance)
        {
            bestDistance = distance;
            bestTarget = targetTransform;
        }
    }

    private void AimAtTarget()
    {
        if (currentTarget == null || firePoint == null)
            return;

        Vector2 direction = (currentTarget.position - firePoint.position).normalized;

        if (rotateTurretVisual && turretVisual != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            turretVisual.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || firePoint == null || currentTarget == null)
            return;

        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile == null)
            return;

        Collider2D projectileCollider = projectileObj.GetComponent<Collider2D>();
        Collider2D[] shooterColliders = GetComponentsInChildren<Collider2D>();

        if (projectileCollider != null)
        {
            foreach (Collider2D col in shooterColliders)
            {
                if (col != null)
                    Physics2D.IgnoreCollision(projectileCollider, col);
            }
        }

        Vector2 directionToTarget = (currentTarget.position - firePoint.position).normalized;
        if (directionToTarget == Vector2.zero)
            directionToTarget = Vector2.right;

        Vector2 velocity;

        if (useArcShot)
        {
            velocity = new Vector2(directionToTarget.x * projectileSpeed, upwardForce);
        }
        else
        {
            velocity = directionToTarget * projectileSpeed;
        }

        projectile.Initialize(velocity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (firePoint != null && currentTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(firePoint.position, currentTarget.position);
        }
    }
}