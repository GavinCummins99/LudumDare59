using System;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    public bool hasTriggered = false; 
    public int messageIndex;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && hasTriggered == false)
        {
            other.gameObject.GetComponent<BarkScript>().DisplayMessage(messageIndex);
            hasTriggered = true;
            Debug.Log("triggered");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && hasTriggered == true)
        {
            other.gameObject.GetComponent<BarkScript>().HideMessage();
        }
    }
}
