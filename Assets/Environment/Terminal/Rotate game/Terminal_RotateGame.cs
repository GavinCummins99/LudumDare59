using UnityEngine;

public class Terminal_RotateGame : MonoBehaviour
{
    void Start()
    {
        RandomizeChildRotation();
    }

    void RandomizeChildRotation()
    {
        foreach (Transform Child in transform)
            Child.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 3) * 90f);
    }

    public void CheckWin()
    {
        foreach (Transform Child in transform)
        {
            if (Child.transform.rotation != Quaternion.identity)
                return;
        }

        Terminal T = GetComponentInParent<Terminal>();
        if (T == null) { Debug.LogError("No Terminal found in parents!"); return; }

        T.GameComplete.Invoke();
        Destroy(transform.root.gameObject);
    }
}