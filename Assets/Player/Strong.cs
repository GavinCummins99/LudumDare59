using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Strong : MonoBehaviour
{
    private Animator Anim;
    private InputAction SpecialAction;
    private Player2D Player;
    private bool OnCooldown = false;
    public float PunchTime = 1.0f;
    public float UnlockTime = 1.97f;
    public float PunchRange = 2f;
    public AudioClip SplosionSound;

    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
        Player = GetComponent<Player2D>();
        SpecialAction = new InputAction("Special", binding: "<Keyboard>/e");
        SpecialAction.performed += _ => DoSpecial();
        SpecialAction.Enable();
    }

    void OnDestroy()
    {
        SpecialAction?.Dispose();
    }

    public void DoSpecial()
    {
        if (OnCooldown) return;
        // Only allow if player is possessed
        if (Player == null || !Player.Possesed) return;

        if (Anim != null)
            Anim.SetTrigger("Special");

        StartCoroutine(Cooldown());
    }

    private bool _switchedAwayMidAction = false;

    public void NotifyUnpossessed()
    {
        _switchedAwayMidAction = true;
    }

    private System.Collections.IEnumerator Cooldown()
    {
        OnCooldown = true;
        _switchedAwayMidAction = false;

        if (Player != null)
            Player.Possesed = false;

        yield return new WaitForSeconds(PunchTime);

        Vector2 Direction = new Vector2(transform.localScale.x, 0f);
        RaycastHit2D[] Hits = Physics2D.RaycastAll(transform.position, Direction, PunchRange);
        foreach (RaycastHit2D Hit in Hits)
        {
            if (Hit.collider.gameObject.CompareTag("Rock"))
            {
                Debug.Log("SMASHHHH");
                GetComponentInChildren<AudioSource>().PlayOneShot(SplosionSound);
                Destroy(Hit.collider.gameObject);
                break;
            }
        }

        yield return new WaitForSeconds(UnlockTime - PunchTime);
        OnCooldown = false;

        // Only re-possess if we weren't switched away from mid-action
        if (Player != null && !_switchedAwayMidAction)
            Player.Possesed = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, new Vector3(transform.localScale.x, 0f, 0f) * PunchRange);
    }
}