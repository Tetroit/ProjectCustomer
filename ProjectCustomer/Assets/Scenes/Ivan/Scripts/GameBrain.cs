
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    GAME,
    MENU,
    SETTINGS,
    INVENTORY,
    DIALOGUE,
    CUTSCENE,
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
    public string gameScene = "amogus";
    public string startCutscene = "intro";
    public string menuScene = "TrueMenu";
    public string titlesScene = "Titles";

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
        RenderSettings.ambientIntensity = 1.0f;
    }

    void Initialise()
    {
        gameState = startState;
        _previousState = startState;
        SetGameState(gameState);

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

    void SetGameState(GameState state)
    {
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
            case GameState.CUTSCENE:
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                }
            }
        }
    public void ChangeGameState(GameState state)
    {
        Debug.Log("Changed game state to " + state);
        if (gameState == state) return;
        _previousState = gameState;
        gameState = state;
        SetGameState(state);
    }
    public void ReturnToPreviousState()
    {
        ChangeGameState(_previousState);
    }
    public void StartCutscene()
    {
        CloseSettings();
        SceneManager.LoadScene(startCutscene);
        ChangeGameState(GameState.CUTSCENE);
    }
    public void StartGame()
    {
        CloseSettings();
        SceneManager.LoadScene(gameScene);
        ChangeGameState(GameState.GAME);
    }
    public void ExitToMenu(bool saveGame = true)
    {
        if (SaveManager.instance && saveGame)
        {
            SaveManager.instance.SaveData();
            SaveManager.instance.WriteData();
        }
        CloseSettings();
        SceneManager.LoadScene("TrueMenu");
        ChangeGameState(GameState.MENU);
    }
    public void ExitToDesktop()
    {
        if (SaveManager.instance)
        {
            SaveManager.instance.SaveData();
            SaveManager.instance.WriteData();
        }
        Application.Quit();
    }
}
