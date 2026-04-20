using UnityEngine;

public class DeathScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player2D>() != null)
        {
            other.GetComponent<Player2D>().ResetCurrentLevel();
        }
    }
}
