using UnityEngine;

public class SignalController : MonoBehaviour
{ 
    public Transform startPoint;
    public Transform endPoint;
    public GameObject LittleDude;
    public GameObject ribbonPrefab;

    private GameObject ribbon;
    private Material mat;

    public void Activate()
    {
        ribbon = Instantiate(ribbonPrefab);
        mat = ribbon.GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (LittleDude)
        {
            Debug.Log("LittleDude");
        }
        if (!startPoint || !LittleDude || !ribbon) return;

        Vector3 start = startPoint.position;
        endPoint = LittleDude.transform.Find("SignalPoint");
        Vector3 end = endPoint.position;

        Vector3 dir = end - start;
        float length = dir.magnitude;

        // midpoint
        ribbon.transform.position = (start + end) * 0.5f;

        // stretch along X
        ribbon.transform.localScale = new Vector3(length, 1f, 0f);

        // rotate to face from start to end
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        ribbon.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // send shader data
        mat.SetVector("_StartPos", start);
        mat.SetVector("_EndPos", end);
    }

    public void Deactivate()
    {
        if (ribbon)
        {
            Destroy(ribbon);
        }
    }
}
