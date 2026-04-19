using UnityEngine;
using System.Collections;

public class Pipe : MonoBehaviour
{
    public Pipe ConnectedPipe;

    public void UsePipe()
    {
        StartCoroutine(PipeSequence());
    }

    private IEnumerator PipeSequence()
    {
        GameObject Agile = GameObject.Find("Agile");
        if (Agile == null) yield break;

        Player2D Player = Agile.GetComponent<Player2D>();

        // Walk to pipe entrance and wait until arrived
        if (Player != null)
        {
            Player.WalkToPoint(transform.position);
            yield return new WaitUntil(() => !Player.IsWalking);
        }

        // Play animation and wait 1.5s
        Animator Anim = Agile.GetComponent<Animator>();
        if (Anim != null)
            Anim.SetTrigger("Special");

        yield return new WaitForSeconds(1.5f);

        // Teleport to connected pipe
        Agile.transform.position = ConnectedPipe.transform.position;
    }
}