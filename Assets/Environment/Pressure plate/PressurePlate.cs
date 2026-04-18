using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PressurePlate : MonoBehaviour
{
    public UnityEvent OnPressed;
    public UnityEvent OnReleased;
    public bool HeavyWeight = false;
    public float WeightThreshold = 2f;

    private List<Player2D> _playersOnPlate = new List<Player2D>();
    private bool _isPressed = false;

    void OnValidate()
    {
        GetComponentInChildren<SpriteRenderer>().color = HeavyWeight ? Color.red : Color.white;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Picmin"))
        {
            Player2D p2d = other.GetComponent<Player2D>();
            if (p2d != null) _playersOnPlate.Add(p2d);
            CheckPlate();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Picmin"))
        {
            Player2D p2d = other.GetComponent<Player2D>();
            if (p2d != null) _playersOnPlate.Remove(p2d);
            CheckPlate();
        }
    }

    void CheckPlate()
    {
        float totalWeight = 0f;
        foreach (Player2D player in _playersOnPlate)
            totalWeight += player.Weight;

        bool shouldPress = HeavyWeight ? totalWeight >= WeightThreshold : _playersOnPlate.Count > 0;

        if (shouldPress && !_isPressed)
        {
            _isPressed = true;
            GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(1, .1f, 1);
            OnPressed.Invoke();
        }
        else if (!shouldPress && _isPressed)
        {
            _isPressed = false;
            GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(1, 1f, 1);
            OnReleased.Invoke();
        }
    }
}