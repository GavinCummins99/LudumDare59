using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    public Texture2D DefaultCursor;
    public Texture2D HoverCursor;
    public Texture2D ClickCursor;
    public Vector2 CursorHotspot = Vector2.zero;
    public float HoverRadius = 0.5f;
    public float FollowSpeed = 10f;
    public float VerticalOffset = 0f;

    private Camera TargetCamera;
    private Texture2D CurrentCursor;
    private GameObject HeldPicmin;
    private Rigidbody2D HeldRigidbody;
    private Player2D Player;

    void Start()
    {
        TargetCamera = FindFirstObjectByType<Camera>();
        Player = GetComponent<Player2D>();
        SetCursor(DefaultCursor);
    }

    void SetCursor(Texture2D Texture)
    {
        if (CurrentCursor == Texture) return;
        CurrentCursor = Texture;
        Cursor.SetCursor(Texture, CursorHotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (TargetCamera == null) return;

        if (Player == null || !Player.Possesed)
        {
            // Not possessed, release anything held and reset cursor
            if (HeldPicmin != null)
            {
                if (HeldRigidbody != null)
                    HeldRigidbody.gravityScale = 1f;
                HeldPicmin = null;
                HeldRigidbody = null;
            }
            SetCursor(DefaultCursor);
            return;
        }

        Vector2 MousePos = Mouse.current.position.ReadValue();
        Vector2 WorldPos = TargetCamera.ScreenToWorldPoint(MousePos);
        WorldPos.y -= VerticalOffset;

        Collider2D Hit = Physics2D.OverlapCircle(TargetCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), HoverRadius);
        bool OverPicmin = Hit != null && Hit.CompareTag("Picmin");

        // Pick up picmin on click
        if (Mouse.current.leftButton.wasPressedThisFrame && OverPicmin && HeldPicmin == null)
        {
            HeldPicmin = Hit.gameObject;
            HeldRigidbody = HeldPicmin.GetComponent<Rigidbody2D>();
            if (HeldRigidbody != null)
            {
                HeldRigidbody.gravityScale = 0f;
                HeldRigidbody.linearVelocity = Vector2.zero;
            }
        }

        // Release picmin on mouse up
        if (Mouse.current.leftButton.wasReleasedThisFrame && HeldPicmin != null)
        {
            if (HeldRigidbody != null)
                HeldRigidbody.gravityScale = 1f;
            HeldPicmin = null;
            HeldRigidbody = null;
        }

        // Smoothly lerp held picmin to mouse position with vertical offset
        if (HeldPicmin != null)
        {
            Vector2 SmoothedPos = Vector2.Lerp(HeldRigidbody.position, WorldPos, Time.deltaTime * FollowSpeed);
            HeldRigidbody.MovePosition(SmoothedPos);
        }

        // Cursor state
        if (Mouse.current.leftButton.isPressed)
            SetCursor(ClickCursor);
        else if (OverPicmin)
            SetCursor(HoverCursor);
        else
            SetCursor(DefaultCursor);
    }
}