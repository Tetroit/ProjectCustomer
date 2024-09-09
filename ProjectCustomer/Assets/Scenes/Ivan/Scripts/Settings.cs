using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public static Settings main;

    public AudioMixer mixer;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    protected float _masterVolume;
    protected float _musicVolume;
    protected float _sfxVolume;

    const string MASTER = "masterVolume";
    const string MUSIC = "musicVolume";
    const string SFX = "sfxVolume";

    //singleton pattern
    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else
            Destroy(this);
    }

    void Start()
    {
        ChangeMasterVolume(masterVolumeSlider.value);
        ChangeMusicVolume(musicVolumeSlider.value);
        ChangeSFXVolume(sfxVolumeSlider.value);

        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ChangeMasterVolume(float value)
    {
        _masterVolume = value * 100 - 80;
        mixer.SetFloat(MASTER, _masterVolume);
    }
    void ChangeMusicVolume(float value)
    {
        _musicVolume = value * 100 - 80;
        mixer.SetFloat(MUSIC, _musicVolume);
    }
    void ChangeSFXVolume(float value)
    {
        _sfxVolume = value * 100 - 80;
        mixer.SetFloat(SFX, _sfxVolume);
    }
}
