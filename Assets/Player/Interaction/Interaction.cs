using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public bool CanInteract = false;
    public string UserName = "";
    public float InteractDistance = 5f;
    public UnityEvent Interacted;
    public GameObject UIElementPrefab;
    public float UIHeightOffset = 1f;

    private InputAction InteractAction;
    private GameObject CurrentUIInstance;
    private Interaction CurrentTarget;
    private Player2D Player;

    void Start()
    {
        if (!CanInteract) return;
        Player = GetComponent<Player2D>();
        InteractAction = new InputAction("Interact", binding: "<Keyboard>/e");
        InteractAction.performed += _ => DoInteract();
        InteractAction.Enable();
    }

    void Update()
    {
        if (!CanInteract) return;
        // Only allow interaction if Player2D is possessed
        if (Player != null && !Player.Possesed)
        {
            if (CurrentUIInstance != null)
            {
                Destroy(CurrentUIInstance);
                CurrentUIInstance = null;
            }
            CurrentTarget = null;
            return;
        }

        Collider2D[] Hits = Physics2D.OverlapCircleAll(transform.position, InteractDistance);
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
        // Block interaction if not possessed
        if (Player != null && !Player.Possesed) return;
        Debug.Log("Invoking on " + CurrentTarget.gameObject.name);
        CurrentTarget.Interacted.Invoke();

        if (gameObject.GetComponent<AudioSource>() != null)
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, InteractDistance);
    }
}