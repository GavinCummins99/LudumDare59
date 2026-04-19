using UnityEngine;
using System.Collections;

public class Pipe : MonoBehaviour
{
    public Pipe ConnectedPipe;
    public bool Invert = false;
    float PositionOffset = -2;

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
        GameObject Agile = GameObject.Find("Agile");
        if (Agile == null) yield break;

        Player2D Player = Agile.GetComponent<Player2D>();

        // Walk to pipe entrance and wait until arrived
        if (Player != null)
        {
            Player.WalkToPoint(transform.position, 1.8f);
            yield return new WaitUntil(() => !Player.IsWalking);
        }

        // Play animation and wait 1.5s
        Animator Anim = Agile.GetComponentInChildren<Animator>();
        if (Anim != null)
            Anim.SetTrigger("Special");

        yield return new WaitForSeconds(1.5f);

        // Teleport to connected pipe with offset based on inversion
        float Direction = ConnectedPipe.Invert ? -1f : 1f;
        Vector3 Offset = new Vector3(Direction * PositionOffset, 0f, 0f);
        Agile.transform.position = ConnectedPipe.transform.position + Offset;
    }
}