using Unity.VisualScripting;
using UnityEngine;

public class TrapDoor : MonoBehaviour {

    public void SetOpen(bool Open) {
        gameObject.SetActive(!Open);
    }

}
