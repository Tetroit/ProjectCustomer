
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
    protected Vector2 _mouseSensitivity;
    public Vector2 mouseSensitivity { get { return _mouseSensitivity; } }

    const string MASTER = "masterVolume";
    const string MUSIC = "musicVolume";
    const string SFX = "sfxVolume";

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

            ChangeMasterVolume(layout.masterVolume.value);
            ChangeMusicVolume(layout.musicVolume.value);
            ChangeSFXVolume(layout.sfxVolume.value);

            ChangeMouseSensitivityX(layout.mouseSensitivityX.value);
            ChangeMouseSensitivityY(layout.mouseSensitivityY.value);

            ToggleFullscreen(layout.fullscreen.isOn);


            layout.masterVolume.onValueChanged.AddListener(ChangeMasterVolume);
            layout.musicVolume.onValueChanged.AddListener(ChangeMusicVolume);
            layout.sfxVolume.onValueChanged.AddListener(ChangeSFXVolume);

            layout.mouseSensitivityX.onValueChanged.AddListener(ChangeMouseSensitivityX);
            layout.mouseSensitivityY.onValueChanged.AddListener(ChangeMouseSensitivityY);

            layout.disableFlashingColors.onValueChanged.AddListener(ToggleFlashingColors);
            layout.displayMode.onValueChanged.AddListener(ChangeDisplayMode);

            layout.exitToMenu.onClick.AddListener(ExitToMenu);
            layout.exitToDesktop.onClick.AddListener(ExitToDesktop);

            layout.fullscreen.onValueChanged.AddListener(ToggleFullscreen);
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
    void ChangeMouseSensitivityX(float value)
    {
        _mouseSensitivity.x = value;
    }
    void ChangeMouseSensitivityY(float value)
    {
        _mouseSensitivity.y = value;
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
}
