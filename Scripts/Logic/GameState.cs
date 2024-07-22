using System.Collections;
using UnityEngine;

public class GameState : MonoBehaviour 
{
    public static GameState Instance { get; private set; }

    public CharacterData selectedCharacter; 
    public int selectedCharacterIndex => savedGameState.selectedCharacterIndex;
    public bool isInitialized {get; private set;}
    
    public string mainMenuSceneName = "MainMenuScene";
    public string playSceneName = "PlayScene";
    
    [Header("Data Containers")]
    [SerializeField]
    public CharacterContainer characterContainer;
    [SerializeField] 
    public WeaponData[] weaponContainer;
    [SerializeField] 
    public LevelContainer levelContainer;
    
    private SavedGameState savedGameState;
    public int selectedLevelIndex => savedGameState.selectedLevelIndex;


    void Awake()
    {
        if (Instance != null)
        {
            //Debug.LogError("Multiple instances of GameState in " + Instance.gameObject.name + " and in " + gameObject.name + "!");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        isInitialized = false;
        Instance = this;
        savedGameState = new SavedGameState();
    }
    
    void Start()
    {
        if(!isInitialized)
            StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(0.1f);  
        SavedGameState loadedGameState = JsonUtility.FromJson<SavedGameState>(PlayerPrefs.GetString("SavedGameState"));
        if (loadedGameState == null)
        {
            loadedGameState = new SavedGameState();
        }
        savedGameState = loadedGameState;
        CharacterChanged(savedGameState.selectedCharacterIndex);
        isInitialized = true;
    }
    
    public void SaveGameState()
    {
        PlayerPrefs.SetString("SavedGameState", JsonUtility.ToJson(savedGameState));
    }

    public void CharacterChanged(int index)
    {
        savedGameState.selectedCharacterIndex = index;
        selectedCharacter = characterContainer.characters[index];
    }
    
    public void LevelChanged(int index)
    {
        savedGameState.selectedLevelIndex = index;
    }
    
    public LevelData GetSelectedLevel()
    {
        return levelContainer.levels[selectedLevelIndex];
    }
}
