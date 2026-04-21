using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TMPButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    int index = 0;
    public Sprite[] sprites;
    public Image spriteRenderer;
    public Sprite Next;


    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //animator.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //animator.enabled = false;
    }
    
    public void LoadFirstLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + 1);
    }

    public void Tutorial()
    {

        if (index == 9)
        {
            LoadFirstLevel();
        }

        GetComponent<Image>().sprite = Next;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(300, -135);
        spriteRenderer.sprite = sprites[index];
        index++;


    }

    public void QuitGame()
    {
        Application.Quit();
    }
}