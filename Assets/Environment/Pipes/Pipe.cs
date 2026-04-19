using UnityEngine;

public class Pipe : MonoBehaviour {
    public Pipe ConnectedPipe;

    public void UsePipe() {
        GameObject Agile = GameObject.Find("Agile");
        if (Agile != null)
            Agile.transform.position = ConnectedPipe.transform.position;
    }
}
