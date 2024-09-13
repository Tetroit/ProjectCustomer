using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public enum GameState
{
    GAME,
    MENU,
    SETTINGS,
    INVENTORY,
    DIALOGUE,
}
public class GameBrain : MonoBehaviour
{
    public static GameBrain main;
    public AudioSource backgroungMusicPlayer = null;
    private string _lastScene;

    [SerializeField]
    public List<AudioClip> playlist;

    public Canvas settingsWindow;
    protected bool _settingsEnabled = false;

    public GameState startState = GameState.MENU;
    public GameState gameState { get; private set;}
    protected GameState _previousState = GameState.GAME;

    private void Awake()
    {
       
    }
    void Start()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
            Initialise();
        }
        else if (main != this)
            Destroy(gameObject);

        if (settingsWindow != null)
        {
            CloseSettings();
        }
    }

    void Initialise()
    {
        gameState = startState;
        _previousState = startState;

        if (settingsWindow == null)
            settingsWindow = GetComponentInChildren<Canvas>();

        //ensure audio player presence
        if (backgroungMusicPlayer == null)
        {
            backgroungMusicPlayer = GetComponent<AudioSource>();
            if (backgroungMusicPlayer == null)
            {
                backgroungMusicPlayer = gameObject.AddComponent<AudioSource>();
            }
        }

        if (playlist.Count > 0)
        {
            backgroungMusicPlayer.loop = true;
            backgroungMusicPlayer.clip = playlist[Random.Range(0, playlist.Count)];
            backgroungMusicPlayer.Play();
        }
    }
    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1.0f;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (settingsWindow != null)
            {
                if (!_settingsEnabled)
                    OpenSettings();
                else
                    CloseSettings();
            }
            else
            {
                Debug.Log("Settings window is not set");
            }
        }
    }

    public void OpenSettings()
    {
        settingsWindow.gameObject.SetActive(true);
        _settingsEnabled = true;
        ChangeGameState(GameState.SETTINGS);
    }
    public void CloseSettings()
    {
        settingsWindow.gameObject.SetActive(false);
        _settingsEnabled = false;
        ChangeGameState(_previousState);
    }

    public void ChangeGameState(GameState state)
    {
        if (gameState == state) return;
        _previousState = gameState;
        gameState = state;

        switch (state)
        {
            case GameState.GAME:
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                }
            case GameState.SETTINGS:
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                }
            case GameState.INVENTORY:
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                }
            case GameState.MENU:
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    break;
                }
        }
    }
    public void ReturnToPreviousState()
    {
        ChangeGameState(_previousState);
    }
    public void StartGame(string scene)
    {
        CloseSettings();
        SceneManager.LoadScene(scene);
        ChangeGameState(GameState.GAME);
    }
    public void ExitToMenu()
    {
        CloseSettings();
        SceneManager.LoadScene("Menu");
        ChangeGameState(GameState.MENU);
    }
    public void ExitToDesktop()
    {
        Application.Quit();
    }
}
