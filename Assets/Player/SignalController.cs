using UnityEngine;

public class SignalController : MonoBehaviour
{ 
    public Transform startPoint;
    public Transform endPoint;
    public GameObject ribbonPrefab;

    private GameObject ribbon;
    private Material mat;

    void Start()
    {
        ribbon = Instantiate(ribbonPrefab);
        mat = ribbon.GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (!startPoint || !endPoint || !ribbon) return;

        Vector3 start = startPoint.position;
        Vector3 end = endPoint.position;

        Vector3 dir = end - start;
        float length = dir.magnitude;

        // midpoint 
        ribbon.transform.position = (start + end) * 0.5f;

        // stretch along X 
        ribbon.transform.localScale = new Vector3(length, 1f, 1f);

        // send shader data
        mat.SetVector("_StartPos", start);
        mat.SetVector("_EndPos", end);
    }
}
