using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public string levelName = null;
    
    //loads level on trigger enter
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SceneTransition>().WipeOut(levelName);
            Debug.Log("WIPEOUT");
        }
    }
    
}
