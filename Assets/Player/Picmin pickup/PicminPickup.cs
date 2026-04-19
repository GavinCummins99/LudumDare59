using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D DefaultCursor;
    public Texture2D HoverCursor;
    public Vector2 CursorHotspot = Vector2.zero;

    private bool IsHovering = false;

    void Start()
    {
        Cursor.SetCursor(DefaultCursor, CursorHotspot, CursorMode.Auto);
    }

    void Update()
    {
        Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D Hit = Physics2D.Raycast(Ray.origin, Ray.direction);

        if (Hit.collider != null && Hit.collider.CompareTag("Picmin"))
        {
            if (!IsHovering)
            {
                Cursor.SetCursor(HoverCursor, CursorHotspot, CursorMode.Auto);
                IsHovering = true;
            }
        }
        else
        {
            if (IsHovering)
            {
                Cursor.SetCursor(DefaultCursor, CursorHotspot, CursorMode.Auto);
                IsHovering = false;
            }
        }
    }
}