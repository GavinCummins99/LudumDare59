using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    public Sprite openTrapDoor;
    public Sprite closedTrapDoor;

    private SpriteRenderer spriteRenderer;
    private bool isOpen;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateDoor();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;   // toggle bool
        UpdateDoor();
    }

    public void SetDoorState(bool Open)
    {
        isOpen = Open;
        UpdateDoor();

    }

    private void UpdateDoor()
    {
        spriteRenderer.sprite = isOpen ? openTrapDoor : closedTrapDoor;

        GetComponent<Collider2D>().enabled = !isOpen;
    }
}