using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour {


    public UnityEvent OnPressed;
    public UnityEvent OnReleased;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Picmin"))
        {
            Debug.Log(other.gameObject.name + " entered the pressure plate");
            this.GetComponentInChildren<SpriteRenderer>().gameObject.transform.localScale = new Vector3(1,.1f,1);
            OnPressed.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Picmin"))
        {
            Debug.Log(other.gameObject.name + " left the pressure plate");
            this.GetComponentInChildren<SpriteRenderer>().gameObject.transform.localScale = new Vector3(1, 1f, 1);
            OnReleased.Invoke();

        }
    }
}
