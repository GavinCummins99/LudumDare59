using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public bool CanInteract = false;
    public string UserName = "";
    public UnityEvent Interacted;
    public GameObject UIElementPrefab;
    public float UIHeightOffset = 1f;

    private InputAction InteractAction;
    private GameObject CurrentUIInstance;
    private Interaction CurrentTarget;

    void Start()
    {
        if (!CanInteract) return;
        InteractAction = new InputAction("Interact", binding: "<Keyboard>/e");
        InteractAction.performed += _ => DoInteract();
        InteractAction.Enable();
    }

    void Update()
    {
        if (!CanInteract) return;

        Collider2D[] Hits = Physics2D.OverlapCircleAll(transform.position, 5f);
        Interaction NearestTarget = null;

        foreach (Collider2D Hit in Hits)
        {
            if (Hit.gameObject == gameObject) continue;
            Interaction Other = Hit.GetComponent<Interaction>();

            // Check UserName on the target - if set, only this gameobject's name can interact with it
            if (Other != null && !Other.CanInteract &&
               (string.IsNullOrEmpty(Other.UserName) || Other.UserName == gameObject.name))
            {
                NearestTarget = Other;
                break;
            }
        }

        if (NearestTarget != null)
        {
            if (CurrentTarget != NearestTarget)
            {
                if (CurrentUIInstance != null)
                {
                    Destroy(CurrentUIInstance);
                    CurrentUIInstance = null;
                }

                CurrentTarget = NearestTarget;

                if (NearestTarget.UIElementPrefab != null)
                {
                    Vector3 SpawnPos = NearestTarget.transform.position + new Vector3(0f, NearestTarget.UIHeightOffset, 0f);
                    CurrentUIInstance = Instantiate(NearestTarget.UIElementPrefab, SpawnPos, Quaternion.identity);
                    CurrentUIInstance.transform.localScale = Vector3.one;
                }
            }
            else if (CurrentUIInstance != null)
            {
                CurrentUIInstance.transform.position = NearestTarget.transform.position + new Vector3(0f, NearestTarget.UIHeightOffset, 0f);
            }
        }
        else
        {
            if (CurrentUIInstance != null)
            {
                Destroy(CurrentUIInstance);
                CurrentUIInstance = null;
            }
            CurrentTarget = null;
        }
    }

    void OnDestroy()
    {
        InteractAction?.Dispose();
        if (CurrentUIInstance != null)
            Destroy(CurrentUIInstance);
    }

    void DoInteract()
    {
        if (CurrentTarget == null) return;
        Debug.Log("Invoking on " + CurrentTarget.gameObject.name);
        CurrentTarget.Interacted.Invoke();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}