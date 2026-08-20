using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int playerIndex = 1;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootCooldown = 0.35f;

    [Header("Fallback Input")]
    [SerializeField] private KeyCode fallbackShootKey = KeyCode.S;
    [SerializeField] private KeyCode fallbackShootKeyPlayer2 = KeyCode.DownArrow;

    [Header("Fire Power")]
    [SerializeField] private float fireDurationRemaining = 0f;
    [SerializeField] private GameObject fireVisual;

    private float cooldownTimer;
    private SpriteRenderer spriteRenderer;

    public bool HasFirePower => fireDurationRemaining > 0f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateFireVisual();
        EnsureInputLoaded();
    }

    private void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
            return;

        EnsureInputLoaded();

        cooldownTimer -= Time.deltaTime;

        if (fireDurationRemaining > 0f)
        {
            fireDurationRemaining -= Time.deltaTime;

            if (fireDurationRemaining <= 0f)
            {
                fireDurationRemaining = 0f;
                UpdateFireVisual();
            }
        }

        if (Input.GetKeyDown(GetShootKey()))
        {
            TryShoot();
        }
    }

    private void EnsureInputLoaded()
    {
        if (InputManager.Player1_Left == KeyCode.None &&
            InputManager.Player1_Right == KeyCode.None &&
            InputManager.Player1_Jump == KeyCode.None &&
            InputManager.Player1_Shoot == KeyCode.None &&
            InputManager.Player2_Left == KeyCode.None &&
            InputManager.Player2_Right == KeyCode.None &&
            InputManager.Player2_Jump == KeyCode.None &&
            InputManager.Player2_Shoot == KeyCode.None)
        {
            InputManager.Load();
        }
    }

    public void GiveFirePower(float duration)
    {
        if (duration <= 0f) return;

        fireDurationRemaining += duration;
        UpdateFireVisual();
    }

    private void TryShoot()
    {
        if (!HasFirePower) return;
        if (cooldownTimer > 0f) return;
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        PlayerProjectile projectile = projectileObj.GetComponent<PlayerProjectile>();
        if (projectile == null) return;

        Vector2 direction = GetShootDirection();
        projectile.Initialize(direction);

        Collider2D projectileCollider = projectileObj.GetComponent<Collider2D>();
        Collider2D[] playerColliders = GetComponentsInChildren<Collider2D>();

        if (projectileCollider != null)
        {
            foreach (Collider2D col in playerColliders)
            {
                if (col != null)
                    Physics2D.IgnoreCollision(projectileCollider, col);
            }
        }

        cooldownTimer = shootCooldown;
    }

    private Vector2 GetShootDirection()
    {
        if (spriteRenderer != null)
            return spriteRenderer.flipX ? Vector2.left : Vector2.right;

        return Vector2.right;
    }

    private KeyCode GetShootKey()
    {
        if (playerIndex == 1)
        {
            return InputManager.Player1_Shoot != KeyCode.None
                ? InputManager.Player1_Shoot
                : fallbackShootKey;
        }
        else
        {
            return InputManager.Player2_Shoot != KeyCode.None
                ? InputManager.Player2_Shoot
                : fallbackShootKeyPlayer2;
        }
    }

    private void UpdateFireVisual()
    {
        if (fireVisual != null)
            fireVisual.SetActive(HasFirePower);
    }

    public float GetFireDurationRemaining()
    {
        return fireDurationRemaining;
    }
}