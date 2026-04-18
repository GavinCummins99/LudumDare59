using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour {
    //public variables
    [Header("Elevator settings"), Range(1,50)]
    public int Height = 3;
    public bool Cyclic = true;
    public bool Vertical = true;

    //Object private variables
    Vector3 StartPoint;
    Vector3 EndPoint;
    GameObject Background;
    GameObject Platform;
    bool UpDirection = true;

    private void OnValidate() {
        //Find children
        Background = transform.Find("Background").gameObject;
        Platform = transform.Find("Platform").gameObject;

        //Set the height of the background texture
        Background.transform.rotation = Vertical ? Quaternion.identity : Quaternion.Euler(0, 0, 90);
        Background.transform.localScale = Vertical ? new Vector3(1, Height, 1) : new Vector3(1, Height, 1);
        Background.transform.position = Vertical ? new Vector3(transform.position.x, transform.position.y + Height / 2f, transform.position.z) : new Vector3(transform.position.x -1.5f + Height / 2f, transform.position.y + 1.5f, transform.position.z);

        //Setup start and end points
        StartPoint = new Vector3(transform.position.x, transform.position.y + .25f, transform.position.z); 
        EndPoint = Vertical ? new Vector3(transform.position.x, transform.position.y - .25f + Height, transform.position.z) : new Vector3(transform.position.x - 3f + Height, transform.position.y + .25f, transform.position.z);

    }

    //Sets the elevator state to moving or not moving 
    public void SetElevatorState(bool Enabled) {
        if (Enabled)
            StartCoroutine(MoveElevator());
        else
            StopAllCoroutines();
    }

    //Moves the elevator up and down, and loops if cyclic is true
    IEnumerator MoveElevator() {
        //Setup variables for movement
        float Elapsed = 0f;
        float Speed = .5f * Height;
        Vector3 Start = Platform.transform.position;

        //Do movement over time
        while (Elapsed < Speed) {
            Elapsed += Time.deltaTime;
            Platform.transform.position = Vector3.Lerp(Start, UpDirection ? EndPoint : StartPoint, Elapsed / Speed);
            yield return null;
        }

        //Snaps to end position to avoid floating point errors
        Platform.transform.position = UpDirection ? EndPoint : StartPoint;

        //Checks if cyclic
        if (Cyclic) {
            UpDirection = !UpDirection;
            StartCoroutine(MoveElevator());
        }
    }
}
