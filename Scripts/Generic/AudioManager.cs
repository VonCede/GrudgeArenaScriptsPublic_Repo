using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioListener mainAudioListener;
    [SerializeField] private AudioSource musicAudioSource;
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            //Debug.LogError("Multiple instances of AudioManager in " + Instance.gameObject.name + " and in " + gameObject.name + "!");
            return;
        }
        Instance = this;
    }

}
