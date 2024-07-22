using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Utility;

public class PlayState : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnPoint;
    public static PlayState Instance { get; private set; }
    public bool IsPaused { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool IsPlayLoaded { get; private set; }
    
    private bool isQuestCompleted;
    
    public Action OnLevelStarted;
    public Action OnLevelEnded;
    

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple instances of PlayState in " + Instance.gameObject.name + " and in " + gameObject.name + "!");
            return;
        }
        Instance = this;
    }
    
    void Start()
    {
        IsPaused = false;
        IsGameOver = false;
        
        // Check if we have a player
        if (GameState.Instance.selectedCharacter == null)
        {
            Debug.LogError("PlayState needs a player!");
        }
        
        // Check if we have a level
        if (GameState.Instance.GetSelectedLevel() == null)
        {
            Debug.LogError("Somehow selected level is null, PlayState needs a level data!");
        }
        
        // Load level visuals
        GameObject loadedLevel = Instantiate(GameState.Instance.GetSelectedLevel().levelPrefab, Vector3.zero, Quaternion.identity);
        
        LevelHelper levelHelper = loadedLevel.GetComponentInChildren<LevelHelper>();
        if (levelHelper != null)
        {
            playerSpawnPoint = levelHelper.playerSpawnPoint;
            if(levelHelper.extractionPoint == null || !levelHelper.extractionPoint.isTrigger)
                Debug.LogError("LevelHelper didn't provide extractionPoint-trigger for PlayState!");
        }
        else
        {
            // Check if we have trigger attached on us
            if (GetComponent<Collider>() == null || !GetComponent<Collider>().isTrigger)
            {
                Debug.LogError("PlayState needs a trigger collider attached to it for extraction point!");
            }
        }
        
        // Check if we have a spawn point
        if (playerSpawnPoint == null)
        {
            Debug.LogError("PlayState needs a player spawn point!");
        }
       
        // Move player to spawn point
        CharacterManager.Instance.playerBase.transform.position = playerSpawnPoint.position;        
        CharacterManager.Instance.playerBase.transform.rotation = playerSpawnPoint.rotation;
        
        Camera.main.GetComponent<SmoothFollow>().SetTarget(CharacterManager.Instance.playerBase.transform);
        
        StartCoroutine(LoadLevelData());
        
    }

    private IEnumerator LoadLevelData()
    {
        // TODO: Load level data
        
        IsPlayLoaded = true;
        
        StartCoroutine(PauseMenu.Instance.FadeOutPlayLoading());
        yield return new WaitForSeconds(PauseMenu.Instance.loadingScreenFadeTime);
        
        // Lets start the game
        OnLevelStarted?.Invoke();
    }

    public void OnEndGame()
    {
        OnLevelEnded?.Invoke();
        //SetPause(true);
        IsGameOver = true;
        PauseMenu.Instance.ShowGameOverPanel();
    }
    
    public void SetPause(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
        IsPaused = pause;
    }

    public void OnGameOver()
    {
        OnEndGame();
        // TODO: Implement this, scoring and stuff
        
        GameState.Instance.SaveGameState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isQuestCompleted)
                OnGameOver();
        }
    }

    public void OnAbortGame()
    {
        OnLevelEnded?.Invoke();
        SetPause(false);
        SceneManager.LoadScene(GameState.Instance.mainMenuSceneName);
    }
}
