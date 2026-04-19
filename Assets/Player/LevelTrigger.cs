using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    public string levelName = null;
   
    
    //loads level on trigger enter
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<SceneTransition>().WipeOut(levelName);
            Debug.Log("WIPEOUT");
        }
    }

    // Update is called once per frame
    void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
