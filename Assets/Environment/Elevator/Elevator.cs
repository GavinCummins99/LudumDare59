using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Elevator settings"), Range(1, 50)]
    public int Height = 3;
    public bool Cyclic = true;
    public bool Vertical = true;
    public bool SwitchDirection = false;

    Vector3 StartPoint;
    Vector3 EndPoint;
    GameObject Background;
    GameObject Platform;
    bool UpDirection = true;

    private void Awake()
    {
        InitializeElevator();
    }

    void InitializeElevator()
    {
        Background = transform.Find("Background").gameObject;
        Platform = transform.Find("Platform").gameObject;

        Background.transform.rotation = Vertical ? Quaternion.identity : Quaternion.Euler(0, 0, 90);
        Background.transform.localScale = Vertical ? new Vector3(1, Height, 1) : new Vector3(1, Height, 1);
        Background.transform.position = Vertical
            ? new Vector3(transform.position.x, transform.position.y + Height / 2f, transform.position.z)
            : new Vector3(transform.position.x - 1.5f + Height / 2f, transform.position.y + 1.5f, transform.position.z);

        StartPoint = new Vector3(transform.position.x, transform.position.y + .25f, transform.position.z);
        EndPoint = Vertical
            ? new Vector3(transform.position.x, transform.position.y - .25f + Height, transform.position.z)
            : new Vector3(transform.position.x - 3f + Height, transform.position.y + .25f, transform.position.z);

        Platform.transform.position = SwitchDirection ? EndPoint : StartPoint;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Guard against OnValidate running before scene objects are ready
        if (transform.Find("Background") == null || transform.Find("Platform") == null) return;
        InitializeElevator();
    }
#endif

    public void SetElevatorState(bool Enabled)
    {
        if (Enabled)
            StartCoroutine(MoveElevator());
        else
            StopAllCoroutines();
    }

    IEnumerator MoveElevator()
    {
        float Elapsed = 0f;
        float Speed = .5f * Vector3.Distance(Platform.transform.position, UpDirection ? EndPoint : StartPoint);
        Vector3 Start = Platform.transform.position;

        while (Elapsed < Speed)
        {
            Elapsed += Time.deltaTime;
            Platform.transform.position = Vector3.Lerp(Start, UpDirection ? EndPoint : StartPoint, Elapsed / Speed);
            yield return null;
        }

        Platform.transform.position = UpDirection ? EndPoint : StartPoint;

        if (Cyclic)
        {
            UpDirection = !UpDirection;
            StartCoroutine(MoveElevator());
        }
    }
}