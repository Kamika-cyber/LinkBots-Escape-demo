using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private int playerIndex = 1;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ice Movement")]
    [SerializeField] private float normalAcceleration = 20f;
    [SerializeField] private float normalDeceleration = 25f;
    [SerializeField] private float iceAcceleration = 6f;
    [SerializeField] private float iceDeceleration = 2f;

    [Header("Fallback Keys")]
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.W;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform visualRoot;
    
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    private float moveInput;
    private bool isGrounded;
    private bool isOnIce;
    private float currentHorizontalSpeed;

    private KeyCode currentLeftKey;
    private KeyCode currentRightKey;
    private KeyCode currentJumpKey;



    private float conveyorSpeed;
    private int conveyorContacts;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        LoadKeys();
    }

    private void Start()
    {
        LoadKeys();
    }

    private void Update()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
            return;

        LoadKeys();

        moveInput = 0f;

        if (Input.GetKey(currentLeftKey))
            moveInput = -1f;

        if (Input.GetKey(currentRightKey))
            moveInput = 1f;

        // if (sr != null)
        // {
        //     if (moveInput > 0f)
        //         sr.flipX = false;
        //     else if (moveInput < 0f)
        //         sr.flipX = true;
        // }

        if (visualRoot != null)
        {
            if (moveInput > 0f)
                visualRoot.localScale = new Vector3(1f, 1f, 1f);
            else if (moveInput < 0f)
                visualRoot.localScale = new Vector3(-1f, 1f, 1f);
        }    

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(currentJumpKey) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (animator != null)
        {
            animator.SetBool("isMoving", Mathf.Abs(moveInput) > 0.01f && isGrounded);
        }
    }

    private void FixedUpdate()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
            return;

        float targetSpeed = moveInput * moveSpeed;

        float acceleration = isOnIce ? iceAcceleration : normalAcceleration;
        float deceleration = isOnIce ? iceDeceleration : normalDeceleration;

        if (Mathf.Abs(moveInput) > 0.01f)
        {
            currentHorizontalSpeed = Mathf.MoveTowards(
                currentHorizontalSpeed,
                targetSpeed,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            currentHorizontalSpeed = Mathf.MoveTowards(
                currentHorizontalSpeed,
                0f,
                deceleration * Time.fixedDeltaTime
            );
        }

        // Добавляем скорость конвейера
        float finalX = currentHorizontalSpeed + conveyorSpeed;
        rb.linearVelocity = new Vector2(finalX, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckIce(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckIce(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ice"))
        {
            isOnIce = false;
        }
    }

    private void CheckIce(Collision2D collision)
    {
        isOnIce = collision.gameObject.CompareTag("Ice");
    }

    private void LoadKeys()
    {
        if (InputManager.Player1_Left == KeyCode.None &&
            InputManager.Player1_Right == KeyCode.None &&
            InputManager.Player1_Jump == KeyCode.None &&
            InputManager.Player2_Left == KeyCode.None &&
            InputManager.Player2_Right == KeyCode.None &&
            InputManager.Player2_Jump == KeyCode.None)
        {
            InputManager.Load();
        }

        if (playerIndex == 1)
        {
            currentLeftKey = InputManager.Player1_Left != KeyCode.None ? InputManager.Player1_Left : leftKey;
            currentRightKey = InputManager.Player1_Right != KeyCode.None ? InputManager.Player1_Right : rightKey;
            currentJumpKey = InputManager.Player1_Jump != KeyCode.None ? InputManager.Player1_Jump : jumpKey;
        }
        else
        {
            currentLeftKey = InputManager.Player2_Left != KeyCode.None ? InputManager.Player2_Left : KeyCode.LeftArrow;
            currentRightKey = InputManager.Player2_Right != KeyCode.None ? InputManager.Player2_Right : KeyCode.RightArrow;
            currentJumpKey = InputManager.Player2_Jump != KeyCode.None ? InputManager.Player2_Jump : KeyCode.UpArrow;
        }
    }

    // ========= МЕТОДЫ ДЛЯ КОНВЕЙЕРА =========

    public void EnterConveyor(float speed)
    {
        conveyorContacts++;
        conveyorSpeed = speed;
    }

    public void StayOnConveyor(float speed)
    {
        conveyorSpeed = speed;
    }

    public void ExitConveyor()
    {
        conveyorContacts = Mathf.Max(0, conveyorContacts - 1);

        if (conveyorContacts == 0)
            conveyorSpeed = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}