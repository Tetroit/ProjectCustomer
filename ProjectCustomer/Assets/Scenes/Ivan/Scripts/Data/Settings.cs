
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public static Settings main;

    public AudioMixer mixer;
    public SettingsLayout layout;

    protected float _masterVolume;
    protected float _musicVolume;
    protected float _sfxVolume;
    public bool isFullscreen { get; private set; }
    protected float _mouseSensitivity;
    public float mouseSensitivity { get { return _mouseSensitivity; } }

    public const string MASTER = "masterVolume";
    public const string MUSIC = "musicVolume";
    public const string SFX = "sfxVolume";

    public const string PLAYER_GROUP = "Player";
    public const string MASTER_GROUP = "Master";
    public const string MUSIC_GROUP = "Music";
    public const string SFX_GROUP = "SFX";


    public AudioMixerGroup GetMixerChannel(string name)
    {
        return mixer.FindMatchingGroups(name)[0];
    }

    Dictionary<int, Vector2Int> screenResolutions;

    //singleton pattern
    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else if(main != this)
            Destroy(this);
    }

    void Start()
    {
        if (layout == null)
            layout = GetComponentInChildren<SettingsLayout>(true);

        if (layout == null) Debug.Log("Layout for settings not found");
        else
        {
            GetScreenProperties();

            //SOUND

            if (layout.masterVolume)
            {
                ChangeMasterVolume(layout.masterVolume.value);
                layout.masterVolume.onValueChanged.AddListener(ChangeMasterVolume);
            }
            if (layout.musicVolume)
            {
                ChangeMusicVolume(layout.musicVolume.value);
                layout.musicVolume.onValueChanged.AddListener(ChangeMusicVolume);
            }
            if (layout.sfxVolume)
            {
                ChangeSFXVolume(layout.sfxVolume.value);
                layout.sfxVolume.onValueChanged.AddListener(ChangeSFXVolume);
            }

            //CONTROLS

            if (layout.mouseSensitivity)
            {
                ChangeMouseSensitivity(layout.mouseSensitivity.value);
                layout.mouseSensitivity.onValueChanged.AddListener(ChangeMouseSensitivity);
            }

            //SCREEN
            if (layout.fullscreen)
            {
                ToggleFullscreen(layout.fullscreen.isOn);
                layout.fullscreen.onValueChanged.AddListener(ToggleFullscreen);
            }
            if (layout.disableFlashingColors)
            {
                layout.disableFlashingColors.onValueChanged.AddListener(ToggleFlashingColors);
            }
            if (layout.displayMode)
            {
                layout.displayMode.onValueChanged.AddListener(ChangeDisplayMode);
            }

            //NAVIGATION

            if (layout.exitToMenu)
            {
                layout.exitToMenu.onClick.AddListener(ExitToMenu);
            }
            if (layout.exitToDesktop)
            {
                layout.exitToDesktop.onClick.AddListener(ExitToDesktop);
            }
            if (layout.closeWindow)
            {
                layout.closeWindow.onClick.AddListener(Close);
            }
        }
    }

    void GetScreenProperties()
    {
        screenResolutions = new Dictionary<int, Vector2Int>();
        List<Resolution> resolutions = Screen.resolutions.ToList();
        List<string> screenNames = new List<string>();

        resolutions.Reverse();

        int i = 0;
        foreach (Resolution resolution in resolutions)
        {
            screenNames.Add("" + resolution.width + "x" + resolution.height);
            screenResolutions.Add(i++, new Vector2Int(resolution.width, resolution.height));
        }

        layout.displayMode.AddOptions(screenNames);
        ChangeDisplayMode(0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void ChangeMasterVolume(float value)
    {
        _masterVolume = value;
        mixer.SetFloat(MASTER, _masterVolume);
    }
    void ChangeMusicVolume(float value)
    {
        _musicVolume = value;
        mixer.SetFloat(MUSIC, _musicVolume);
    }
    void ChangeSFXVolume(float value)
    {
        _sfxVolume = value;
        mixer.SetFloat(SFX, _sfxVolume);
    }
    void ChangeMouseSensitivity(float value)
    {
        _mouseSensitivity = value;
    }
    void ExitToMenu()
    {
        GameBrain.main.ExitToMenu();
    }
    void ExitToDesktop()
    {
        GameBrain.main.ExitToDesktop();
    }
    void ToggleFlashingColors(bool value)
    {
        if (value) GameBrain.main.GetComponent<NoiseManager>().DisableNoise();
        else GameBrain.main.GetComponent<NoiseManager>().EnableNoise();
    }
    void ToggleFullscreen(bool value)
    {
        isFullscreen = value;
        Screen.fullScreen = value;
    }
    void ChangeDisplayMode(int mode)
    {
        Vector2Int res = screenResolutions[mode];
        Screen.SetResolution(res.x, res.y, isFullscreen);
    }
    void Close()
    {
        GameBrain.main.CloseSettings();
    }
}
