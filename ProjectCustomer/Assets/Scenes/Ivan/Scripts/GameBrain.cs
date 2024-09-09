using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBrain : MonoBehaviour
{
    public static GameBrain main;
    public AudioSource backgroungMusicPlayer = null;
    public List<AudioClip> playlist;

    public Canvas settingsWindow;
    protected bool _settingsEnabled = false;

    //singleton pattern
    private void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
    }
    void Start()
    {
        if (settingsWindow != null)
        {
            DontDestroyOnLoad(settingsWindow);
            CloseSettings();
        }
        //ensure audio player presence
        if (backgroungMusicPlayer == null)
        {
            backgroungMusicPlayer = GetComponent<AudioSource>();
            if (backgroungMusicPlayer == null ) 
            {
                backgroungMusicPlayer = gameObject.AddComponent<AudioSource>();
            }
        }

        backgroungMusicPlayer.loop = true;
        backgroungMusicPlayer.clip = playlist[Random.Range(0, playlist.Count)];
        backgroungMusicPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1.0f;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_settingsEnabled)
            {
                Debug.Log("openSettings");
                OpenSettings();
            }
            else
            {
                Debug.Log("closeSettings");
                CloseSettings();
            }
        }
    }

    public void OpenSettings()
    {
        settingsWindow.gameObject.SetActive(true);
        _settingsEnabled = true;
    }
    public void CloseSettings()
    {
        settingsWindow.gameObject.SetActive(false);
        _settingsEnabled = false;
    }
}
