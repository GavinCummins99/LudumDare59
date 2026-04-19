using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Terminal_Slot : MonoBehaviour
{

    bool CanClick = true;

    void Start()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(_ => OnClicked());
        trigger.triggers.Add(entry);
    }

    void OnClicked()
    {
        if (!CanClick) return;
        StartCoroutine(RotateTo(transform.eulerAngles.z - 90f, 0.3f));
    }

    IEnumerator RotateTo(float TargetZ, float Duration)
    {
        CanClick = false;
        float Elapsed = 0f;
        float StartZ = transform.eulerAngles.z;

        while (Elapsed < Duration)
        {
            Elapsed += Time.deltaTime;
            float Z = Mathf.LerpAngle(StartZ, TargetZ, Elapsed / Duration);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Z);
            yield return null;
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, TargetZ);
        CanClick = true;
        transform.parent.GetComponent<Terminal_RotateGame>().CheckWin();
    }
}