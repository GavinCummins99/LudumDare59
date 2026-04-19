using UnityEngine;

public class Pipe : MonoBehaviour
{
    public Pipe ConnectedPipe;

    public void UsePipe()
    {
        StartCoroutine(TeleportDelay());
    }

    private System.Collections.IEnumerator TeleportDelay()
    {
        GameObject Agile = GameObject.Find("Agile");
        if (Agile != null)
        {
            Animator Anim = Agile.GetComponent<Animator>();
            if (Anim != null)
                Anim.SetTrigger("Special");
        }

        yield return new WaitForSeconds(1.5f);

        if (Agile != null)
        {
            Agile.transform.position = ConnectedPipe.transform.position;
        }
    }
}