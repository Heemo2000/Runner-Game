using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    [SerializeField]private Slider musicVolumeSlider;
    [SerializeField]private Slider sfxVolumeSlider;

    
    private void Awake() 
    {
        musicVolumeSlider.onValueChanged.AddListener(SoundManager.Instance.SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SoundManager.Instance.SetSFXVolume);
    }

    private void OnDestroy() 
    {
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.RemoveAllListeners();    
    }
}
