using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public bool CanInteract = false;
    public UnityEvent Interacted;

    private InputAction InteractAction;

    void Start()
    {
        if (!CanInteract) return;

        InteractAction = new InputAction("Interact", binding: "<Keyboard>/e");
        InteractAction.performed += _ => DoInteract();
        InteractAction.Enable();
    }

    void OnDestroy()
    {
        InteractAction?.Dispose();
    }

    void DoInteract()
    {
        Debug.Log("DoInteract called on " + gameObject.name);

        Collider2D[] Hits = Physics2D.OverlapCircleAll(transform.position, 5f);

        foreach (Collider2D Hit in Hits)
        {
            if (Hit.gameObject == gameObject) continue;

            Interaction Other = Hit.GetComponent<Interaction>();
            if (Other != null)
            {
                Debug.Log("Invoking on " + Other.gameObject.name);
                Other.Interacted.Invoke();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}