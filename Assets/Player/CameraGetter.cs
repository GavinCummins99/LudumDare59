using UnityEngine;

public class CameraGetter : MonoBehaviour
{
    private void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        Debug.Log("Camera assigned: " + canvas.worldCamera);
    }
}
