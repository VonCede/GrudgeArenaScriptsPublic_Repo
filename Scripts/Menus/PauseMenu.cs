using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] public float loadingScreenFadeTime = 0.5f;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private CanvasGroup gameOverScreenCanvasGroup;
    [SerializeField] private float gameOverScreenFadeTime = 0.5f;
    
    public static PauseMenu Instance { get; private set; }
    
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of PauseMenu in " + Instance.gameObject.name + " and in " + gameObject.name + "!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenuPanel.SetActive(false);
        gameOverScreenCanvasGroup.gameObject.SetActive(false);
        gameOverScreenCanvasGroup.alpha = 0f;
        loadingScreenCanvasGroup.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(PlayState.Instance.IsGameOver)
            {
                 OnMainMenuButtonClicked();
            }
            if (PlayState.Instance.IsPaused)
            {
                OnResumeButtonClicked();
            }
            else
            {
                OnPauseButtonClicked();
            }
        }
    }
    
    public void OnPauseButtonClicked()
    {
        pauseMenuPanel.SetActive(true);
        PlayState.Instance.SetPause(true);
    }
    
    public void OnResumeButtonClicked()
    {
        pauseMenuPanel.SetActive(false);
        PlayState.Instance.SetPause(false);
    }
    
    public void OnMainMenuButtonClicked()
    {
        PlayState.Instance.OnAbortGame();
    }
    
    public IEnumerator FadeOutPlayLoading()
    {
        float t = 0f;
        while (t < loadingScreenFadeTime)
        {
            t += Time.deltaTime;
            loadingScreenCanvasGroup.alpha = 1f - t / loadingScreenFadeTime;
            yield return null;
        }
        loadingScreenCanvasGroup.gameObject.SetActive(false);
        // reset loading screen alpha
        loadingScreenCanvasGroup.alpha = 1f;
    }

    public void ShowGameOverPanel()
    {
        pauseMenuPanel.SetActive(false);
        gameOverScreenCanvasGroup.gameObject.SetActive(true);
        // Fade in game over panel
        StartCoroutine(FadeInGameOverPanel());
    }
    
    // implement FadeInGameOverPanel
    private IEnumerator FadeInGameOverPanel()
    {
        float t = 0f;
        while (t < gameOverScreenFadeTime)
        {
            t += Time.deltaTime;
            gameOverScreenCanvasGroup.alpha = t / gameOverScreenFadeTime;
            yield return null;
        }
        gameOverScreenCanvasGroup.alpha = 1f;
    }
}
