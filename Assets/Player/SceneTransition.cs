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
        WipeIn(0);
    }

    public void WipeIn(int levelCount) => StartWipe(0f, 1f, levelCount);
    public void WipeOut(int levelCount) => StartWipe(1f, 0f, levelCount);

    public void SetProgress(float value)
    {
        if (irisMaterial != null)
            irisMaterial.SetFloat(ProgressID, Mathf.Clamp01(value));
    }

    private void StartWipe(float from, float to, int levelCount)
    {
        if (_activeWipe != null) StopCoroutine(_activeWipe);
        _activeWipe = StartCoroutine(WipeRoutine(from, to, () =>
        {
            if (levelCount != 0)
            {
                int currentIndex = SceneManager.GetActiveScene().buildIndex;
                int totalScenes = SceneManager.sceneCountInBuildSettings;

                int nextIndex = (currentIndex + 1) % totalScenes;

                SceneManager.LoadScene(nextIndex);
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