using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public int levelCount = 1;
    
    //loads level on trigger enter
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SceneTransition>().WipeOut(levelCount);
            Debug.Log("WIPEOUT");
        }
    }
    
}
