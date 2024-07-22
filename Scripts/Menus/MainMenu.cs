using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject splashScreenPanel;
    [SerializeField] private float minimumSplashShowTime = 3f;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private GameObject creditsMenuPanel;
    
    [Header("Charater Selection")]
    [SerializeField] private GameObject characterSelectionPanel;
    [SerializeField] private GameObject characterSelectionUIPrefab;
    [SerializeField] private CharacterDescriptionUI characterDescriptionUI;
    
    [Header("Level Selection")]
    [SerializeField] private GameObject levelSelectionPanel;
    [SerializeField] private GameObject levelSelectionUIPrefab;
    [SerializeField] private LevelDescriptionUI levelDescriptionUI;
    
    private Action<bool> OnCharacterDeselected;
    private Action<bool> OnLevelDeselcted;
    
    public static MainMenu Instance { get; private set; }
    
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of MainMenu in " + Instance.gameObject.name + " and in " + gameObject.name + "!");
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {  
        splashScreenPanel.SetActive(false);
        optionsMenuPanel.SetActive(false);
        creditsMenuPanel.SetActive(false);
        if(!GameState.Instance.isInitialized)
            StartCoroutine(StartGameCoroutine());
        else
            MakeMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenuPanel.activeSelf)
            {
                OnOptionsCloseButtonClicked();
            }
            else if (creditsMenuPanel.activeSelf)
            {
                OnCreditsCloseButtonClicked();
            }
        }
    }

    private IEnumerator StartGameCoroutine()
    {
        splashScreenPanel.SetActive(true);
        yield return new WaitForSeconds(minimumSplashShowTime);
        while(GameState.Instance.isInitialized == false)
        {
            yield return new WaitForSeconds(0.5f);
            // TODO: Add message to user that game is talking to server
            // TODO: Handle timeout
        }
        MakeMainMenu();
        splashScreenPanel.SetActive(false);
    }

    private void MakeMainMenu()
    {
        mainMenuPanel.SetActive(true);
        // destroy all childs from CharacterSelectionUI in case there were any from development
        foreach (Transform child in characterSelectionPanel.transform)
        {
            Destroy(child.gameObject);
        }
        
        // destroy all childs from LevelSelectionUI in case there were any from development
        foreach (Transform child in levelSelectionPanel.transform)
        {
            Destroy(child.gameObject);
        }
        
        characterDescriptionUI.CharacterChanged();
        for(int i = 0; i < GameState.Instance.characterContainer.characters.Count; i++)
        {
            var characterData = GameState.Instance.characterContainer.characters[i];
            var characterSelectionUI = Instantiate(characterSelectionUIPrefab, characterSelectionPanel.transform);
            characterSelectionUI.GetComponent<CharacterSelectionUI>().Initialize(characterData, i, i == GameState.Instance.selectedCharacterIndex, OnCharacterSelectionButtonClicked);
        }
        
        levelDescriptionUI.LevelChanged();
        for(int i = 0; i < GameState.Instance.levelContainer.levels.Count; i++)
        {
            var levelData = GameState.Instance.levelContainer.levels[i];
            var levelSelectionUI = Instantiate(levelSelectionUIPrefab, levelSelectionPanel.transform);
            levelSelectionUI.GetComponent<LevelSelectionUI>().Initialize(levelData, i, i == GameState.Instance.selectedLevelIndex, OnLevelSelectionButtonClicked);
        }
    }
    
    public void OnPlayButtonClicked()
    {
        mainMenuPanel.SetActive(false);
        GameState.Instance.SaveGameState();
        SceneManager.LoadScene(GameState.Instance.playSceneName);
    }

    public void OnOptionsButtonClicked()
    {
        optionsMenuPanel.SetActive(true);
    }
    
    public void OnOptionsCloseButtonClicked()
    {
        optionsMenuPanel.SetActive(false);
    }
    
    public void OnCreditsButtonClicked()
    {
        creditsMenuPanel.SetActive(true);
    }
    
    public void OnCreditsCloseButtonClicked()
    {
        creditsMenuPanel.SetActive(false);
    }
    
    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
    
    public void OnCharacterSelectionButtonClicked(Action<bool> onUnselected, int index)
    {
        if (OnCharacterDeselected != null)
        {
            OnCharacterDeselected(false);
        }
        OnCharacterDeselected = onUnselected;
        GameState.Instance.CharacterChanged(index);
        characterDescriptionUI.CharacterChanged();
    }  
    
    public void OnLevelSelectionButtonClicked(Action<bool> onUnselected, int index)
    {
        if(OnLevelDeselcted != null)
        {
            OnLevelDeselcted(false);
        }
        GameState.Instance.LevelChanged(index);
        levelDescriptionUI.LevelChanged();
    }
}
