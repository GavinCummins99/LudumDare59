using System;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class BarkScript : MonoBehaviour
{
    public GameObject Canvas;
    public TextMeshProUGUI textBox;
    public string[] messages;

    /*void Start()
    {
        if (gameObject.CompareTag("Player"))
        {
            Canvas = GetComponentInChildren<Canvas>(true).gameObject;
            textBox = Canvas.GetComponentInChildren<TextMeshProUGUI>(true);
        }
    }*/
    
    public void DisplayMessage(int i)
    {
        if (messages != null && i >= 0 && i < messages.Length) 
        {
                textBox.text = messages[i];
                textBox.gameObject.SetActive(true);
        }
    }
    
    public void HideMessage()
    {
        if(textBox.gameObject.activeInHierarchy)
        {
                textBox.gameObject.SetActive(false);
                textBox.text = null;
            
        }
    }
}
