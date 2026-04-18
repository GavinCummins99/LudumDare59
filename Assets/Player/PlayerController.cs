using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player2D : MonoBehaviour
{

    public bool Possesed = false;

    [Header("Input")]
    public InputActionAsset inputActions;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputActionMap playerMap;

    [Header("Movement")]
    public float speed = 7f;
    public float jumpForce = 12f;

    [Header("Jump Feel")]
    public float coyoteTime = 0.12f;
    public float jumpBufferTime = 0.15f;
    public float jumpCutMultiplier = 0.5f;
    public float fallGravityMultiplier = 2.5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    // State
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isGrounded;

    // Timers / flags
    private float coyoteTimer;
    private float jumpBufferTimer;
    private float defaultGravityScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;

        playerMap = inputActions.FindActionMap("Player");

        moveAction = playerMap.FindAction("Move");
        jumpAction = playerMap.FindAction("Jump");

        // Horizontal input
        moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        moveAction.canceled  += _   => moveInput = Vector2.zero;

        // Buffer the jump press
        jumpAction.performed += _ => jumpBufferTimer = jumpBufferTime;

        // Jump-cut: releasing early cuts vertical speed
        jumpAction.canceled += _ =>
        {
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
        };
    }

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void OnEnable()  => playerMap.Enable();
    void OnDisable() => playerMap.Disable();

    void FixedUpdate()
    {
        if (!Possesed) return;
        Debug.Log("print");

        // ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // coyote time: reset when grounded, count down when in air
        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.fixedDeltaTime;

        // tick the jump buffer down every physics step
        jumpBufferTimer -= Time.fixedDeltaTime;

        // horizontal movement
        rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

        // flip sprite to face movement direction
        if (moveInput.x != 0f)
            //transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1f, 1f);

        // jump
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferTimer = 0f;  // consume the buffered input
            coyoteTimer     = 0f;  // prevent double-jumping on coyote window
        }

        // heavier gravity on the way down, feels better imo
        if (!isGrounded && rb.linearVelocity.y < 0f)
            rb.gravityScale = defaultGravityScale * fallGravityMultiplier;
        else
            rb.gravityScale = defaultGravityScale;
    }
}