using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : GenericSingleton<SoundManager>
{
    [SerializeField]private AudioSource musicAudioSource;
    [SerializeField]private AudioSource sfxAudioSource;

    protected override void Awake() {
        base.Awake();
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }
}
