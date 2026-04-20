using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private RawImage irisOverlay;
    [SerializeField] private Material irisMaterial;
    [SerializeField] private float wipeDuration = 1f;
    

    private static readonly int ProgressID = Shader.PropertyToID("_Progress");
    private Coroutine _activeWipe;

    private void Awake()
    {
        irisMaterial = Instantiate(irisOverlay.material);
        irisOverlay.material = irisMaterial;
        SetProgress(0f);
    }

    private void Start()
    {
        WipeIn();
    }

    public void WipeIn(string levelName = null) => StartWipe(0f, 1f, levelName);
    public void WipeOut(string levelName = null) => StartWipe(1f, 0f, levelName);

    public void SetProgress(float value)
    {
        if (irisMaterial != null)
            irisMaterial.SetFloat(ProgressID, Mathf.Clamp01(value));
    }

    private void StartWipe(float from, float to, string levelName = null)
    {
        if (_activeWipe != null) StopCoroutine(_activeWipe);
        _activeWipe = StartCoroutine(WipeRoutine(from, to, () =>
        {
            if (!string.IsNullOrEmpty(levelName))
            {
                int currentIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(currentIndex + 1);
            }
        }));
    }

    private IEnumerator WipeRoutine(float from, float to, System.Action onComplete)
    {
        float elapsed = 0f;

        while (elapsed < wipeDuration)
        {
            elapsed += Time.deltaTime;
            SetProgress(Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / wipeDuration)));
            yield return null;
        }

        SetProgress(to);
        onComplete?.Invoke();
        _activeWipe = null;
    }
}