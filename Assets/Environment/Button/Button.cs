using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class Button : MonoBehaviour
{
    public bool Invert = false;
    float PositionOffset = -2;
    public UnityEvent ButtonPressed;

    private void OnValidate()
    {
        gameObject.transform.localScale = Invert ? new Vector3(-1, 1, 1) : Vector3.one;
    }

    public void UsePipe()
    {
        StartCoroutine(PipeSequence());
    }

    private IEnumerator PipeSequence()
    {
        GameObject Captain = GameObject.Find("Captain");
        if (Captain == null) yield break;

        Player2D Player = Captain.GetComponent<Player2D>();

        // Walk to button entrance and wait until arrived
        if (Player != null)
        {
            Player.WalkToPoint(transform.position, 1.8f);
            yield return new WaitUntil(() => !Player.IsWalking);
        }

        // Play animation and wait 1.5s
        Animator Anim = Captain.GetComponentInChildren<Animator>();
        if (Anim != null)
            Anim.SetTrigger("Special");

        yield return new WaitForSeconds(.5f);

        ButtonPressed.Invoke();


    }
}