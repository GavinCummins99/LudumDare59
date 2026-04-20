using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TMPButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.enabled = false;
    }
    
    public void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}