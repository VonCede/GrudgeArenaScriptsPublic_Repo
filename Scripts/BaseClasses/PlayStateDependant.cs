using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class PlayStateDependant : MonoBehaviour
{
    protected bool isPlaying => PlayState.Instance.IsPlayLoaded && !PlayState.Instance.IsPaused && !PlayState.Instance.IsGameOver;

    // Start is called before the first frame update
    void Start()
    {
        PlayState.Instance.OnLevelStarted += OnLevelStarted;
        PlayState.Instance.OnLevelEnded += OnLevelEnded;
        DerivedStart();
    }

    protected abstract void DerivedStart();

    private void OnLevelStarted()
    {
        //isPlaying = true;
    }

    private void OnLevelEnded()
    {
        //isPlaying = false;
    }

}
