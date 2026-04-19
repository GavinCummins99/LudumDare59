using UnityEngine;
using UnityEngine.Events;

public class Terminal : MonoBehaviour
{
    public GameObject TerminalUI;
    public GameObject[] Games;
    public UnityEvent GameComplete;

    void CreateGame(Transform parent)
    {
        GameObject SpawnedGame = Instantiate(Games[0], parent);
        SpawnedGame.transform.localScale = Vector3.one;
        SpawnedGame.transform.localPosition = Vector3.zero;
    }

    public void OpenTerminal()
    {
        GameObject SpawnedTerminal = Instantiate(TerminalUI, transform);
        SpawnedTerminal.transform.localScale = Vector3.one;
        SpawnedTerminal.transform.localPosition = Vector3.zero;

        CreateGame(SpawnedTerminal.transform.Find("Pivot").transform);
    }
}
