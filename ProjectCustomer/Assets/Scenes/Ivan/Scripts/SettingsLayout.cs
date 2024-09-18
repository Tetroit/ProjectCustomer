using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsLayout : MonoBehaviour
{
    public Slider masterVolume;
    public Slider musicVolume;
    public Slider sfxVolume;

    public Slider mouseSensitivity;

    public Button closeWindow;
    public Button exitToMenu;
    public Button exitToDesktop;

    public Toggle disableFlashingColors;
    public Toggle fullscreen;
    public TMP_Dropdown displayMode;
}
