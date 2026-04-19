using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    public Texture2D DefaultCursor;
    public Texture2D HoverCursor;
    public Texture2D ClickCursor;
    public Vector2 CursorHotspot = Vector2.zero;
    public float HoverRadius = 0.5f;

    private Camera TargetCamera;
    private Texture2D CurrentCursor;

    void Start()
    {
        TargetCamera = FindAnyObjectByType<Camera>();
        SetCursor(DefaultCursor);
    }

    void SetCursor(Texture2D Texture)
    {
        if (CurrentCursor == Texture) return;
        CurrentCursor = Texture;
        //Debug.Log("Setting cursor to: " + Texture);
        Cursor.SetCursor(Texture, CursorHotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (TargetCamera == null) return;

        Vector2 MousePos = Mouse.current.position.ReadValue();
        Vector2 WorldPos = TargetCamera.ScreenToWorldPoint(MousePos);

        Collider2D Hit = Physics2D.OverlapCircle(WorldPos, HoverRadius);
        bool OverPicmin = Hit != null && Hit.CompareTag("Picmin");

        if (Mouse.current.leftButton.isPressed)
            SetCursor(ClickCursor);
        else if (OverPicmin)
            SetCursor(HoverCursor);
        else
            SetCursor(DefaultCursor);
    }
}