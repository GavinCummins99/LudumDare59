using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Button : MonoBehaviour
{
    public UnityEvent OnPressed;
    public UnityEvent OnReleased;


    //Called when other character enters box trigger
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") || other.CompareTag("Picmin")) {
            Player2D p2d = other.GetComponent<Player2D>();
        }
    }

    //Called when other character enters box trigger
    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") || other.CompareTag("Picmin")) {
            Player2D p2d = other.GetComponent<Player2D>();
        }
    }
}