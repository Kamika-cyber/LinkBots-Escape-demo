using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LinkController : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;

    [Header("Distance Settings")]
    [SerializeField] private float maxDistance = 6f;
    [SerializeField] private float pullStrength = 8f;
    [SerializeField] private float hardClampDistance = 7f;

    [Header("Visual Settings")]
    [SerializeField] private float lineWidth = 0.15f;
    [SerializeField] private float verticalOffset = 0.3f;
    [SerializeField] private Color normalColor = Color.cyan;
    [SerializeField] private Color stretchedColor = Color.red;

    [Header("Laser Link")]
    [SerializeField] private bool enableLaserDamage = false;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float laserCheckRadius = 0.1f;
    [SerializeField] private bool requireClearLineOfSight = true;
    [SerializeField] private bool laserOnlyWhenStretched = true;
    [SerializeField] private float laserActivationThreshold = 0.8f; // 80%

    private LineRenderer lineRenderer;
    private Rigidbody2D rb1;
    private Rigidbody2D rb2;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (player1 != null) rb1 = player1.GetComponent<Rigidbody2D>();
        if (player2 != null) rb2 = player2.GetComponent<Rigidbody2D>();

        SetupLine();
    }

    private void Update()
    {
        if (player1 == null || player2 == null) return;

        DrawLink();
        UpdateVisualState();

        if (enableLaserDamage)
        {
            float distance = Vector2.Distance(player1.position, player2.position);

            bool canUseLaser = true;

            if (laserOnlyWhenStretched)
            {
                float thresholdDistance = maxDistance * laserActivationThreshold;
                canUseLaser = distance >= thresholdDistance;
            }

            if (canUseLaser)
            {
                DamageEnemiesBetweenPlayers();
            }
        }
    }

    private void FixedUpdate()
    {
        if (player1 == null || player2 == null) return;
        if (rb1 == null || rb2 == null) return;

        ApplyLinkConstraint();
    }

    private void SetupLine()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.startColor = normalColor;
        lineRenderer.endColor = normalColor;
    }

    private void DrawLink()
    {
        Vector3 p1 = player1.position + Vector3.up * verticalOffset;
        Vector3 p2 = player2.position + Vector3.up * verticalOffset;

        lineRenderer.SetPosition(0, p1);
        lineRenderer.SetPosition(1, p2);
    }

    private void UpdateVisualState()
    {
        float distance = Vector2.Distance(player1.position, player2.position);
        float t = Mathf.InverseLerp(maxDistance * 0.7f, maxDistance, distance);

        Color currentColor = Color.Lerp(normalColor, stretchedColor, t);

        float pulseWidth = lineWidth + Mathf.Sin(Time.time * 6f) * 0.01f * t;

        lineRenderer.startColor = currentColor;
        lineRenderer.endColor = currentColor;
        lineRenderer.startWidth = pulseWidth;
        lineRenderer.endWidth = pulseWidth;
    }

    private void ApplyLinkConstraint()
    {
        Vector2 p1 = player1.position;
        Vector2 p2 = player2.position;

        float distance = Vector2.Distance(p1, p2);

        if (distance > maxDistance)
        {
            Vector2 direction = (p2 - p1).normalized;
            float extraDistance = distance - maxDistance;

            Vector2 force = direction * extraDistance * pullStrength;

            rb1.AddForce(force);
            rb2.AddForce(-force);
        }

        if (distance > hardClampDistance)
        {
            Vector2 center = (p1 + p2) / 2f;
            Vector2 direction = (p2 - p1).normalized;

            player1.position = center - direction * (hardClampDistance / 2f);
            player2.position = center + direction * (hardClampDistance / 2f);

            rb1.linearVelocity = new Vector2(rb1.linearVelocity.x * 0.5f, rb1.linearVelocity.y);
            rb2.linearVelocity = new Vector2(rb2.linearVelocity.x * 0.5f, rb2.linearVelocity.y);
        }
    }

    private void DamageEnemiesBetweenPlayers()
    {
        Vector2 start = player1.position + Vector3.up * verticalOffset;
        Vector2 end = player2.position + Vector3.up * verticalOffset;
        Vector2 direction = end - start;
        float distance = direction.magnitude;

        if (distance <= 0.01f) return;

        direction.Normalize();

        Collider2D[] hits = Physics2D.OverlapCapsuleAll(
            (start + end) * 0.5f,
            new Vector2(distance, laserCheckRadius * 2f),
            CapsuleDirection2D.Horizontal,
            GetLineAngle(start, end),
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            if (hit == null) continue;

            if (requireClearLineOfSight)
            {
                Vector2 enemyPoint = hit.bounds.center;
                RaycastHit2D blockHit = Physics2D.Linecast(start, enemyPoint, obstacleLayer);

                if (blockHit.collider != null)
                    continue;
            }

            LinkEnemyTarget enemy = hit.GetComponent<LinkEnemyTarget>();
            if (enemy != null)
            {
                enemy.DieFromLink();
            }
        }
    }

    private float GetLineAngle(Vector2 start, Vector2 end)
    {
        Vector2 dir = end - start;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    private void OnDrawGizmosSelected()
    {
        if (player1 == null || player2 == null) return;

        Vector3 p1 = player1.position + Vector3.up * verticalOffset;
        Vector3 p2 = player2.position + Vector3.up * verticalOffset;

        Gizmos.color = enableLaserDamage ? Color.red : Color.cyan;
        Gizmos.DrawLine(p1, p2);
    }
}