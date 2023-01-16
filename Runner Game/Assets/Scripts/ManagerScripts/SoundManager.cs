using System;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : GenericSingleton<SoundManager>
{
    
    [SerializeField]private AudioSource musicAudioSource;
    [SerializeField]private AudioSource sfxAudioSource;
    [SerializeField]private Sound[] sounds;

    [SerializeField]private SoundType startingMusic;
    [SerializeField]private AudioMixer musicAudioMixer;
    [SerializeField]private AudioMixer sfxAudioMixer;
    private Dictionary<SoundType,Sound> _soundDictionary;
    protected override void Awake() {
        
        base.Awake();
        DontDestroyOnLoad(gameObject);
        _soundDictionary = new Dictionary<SoundType, Sound>();        
        foreach(Sound sound in sounds)
        {
            _soundDictionary.Add(sound.soundType,sound);
        }
    }

    private void Start() 
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        Play(SoundType.Music,musicAudioSource);
    }

    public void PlaySFX(SoundType soundType)
    {
        Play(soundType,sfxAudioSource);
    }
    
    public void PlaySFXInstantly(SoundType soundType)
    {
        Play(soundType,sfxAudioSource,true);
    }

    public void PauseMusic()
    {
        musicAudioSource.Pause();
    }

    public void ResumeMusic()
    {
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }


    public void ResumeSFX()
    {
        sfxAudioSource.Play();
    }

    public void StopSFX()
    {
        sfxAudioSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioMixer.SetFloat("musicVol",volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioMixer.SetFloat("sfxVol",volume);
    }
    private void Play(SoundType soundType,AudioSource source,bool oneShot = false)
    {
        if(_soundDictionary.TryGetValue(soundType,out Sound sound))
        {
            source.clip = sound.clip;
            source.pitch = sound.pitch;
            source.loop = sound.loop;
            source.volume = sound.volume;
            if(oneShot)
            {
                source.PlayOneShot(sound.clip);
            }
            else
            {
                source.Play();
            }
            
        }
        else
        {
            Debug.Log("Sound to play not found!!");
        }        
    }
}
