using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Terminal_Slot : MonoBehaviour
{
    bool CanClick = true;

    void Start()
    {
        EventTrigger Trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry Entry = new EventTrigger.Entry();
        Entry.eventID = EventTriggerType.PointerClick;
        Entry.callback.AddListener(_ => OnClicked());
        Trigger.triggers.Add(Entry);
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
        GetComponentInParent<Terminal_RotateGame>().CheckWin();
    }
}