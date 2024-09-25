using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public string fileName = "amogus.json";
    public static SaveManager instance;
    public List<ISaveData> listeners;
    public GameData gameData = new GameData();
    public bool newSave = false;

    private void Awake()
    {
        if (instance == null)
        {
            newSave = ReadData();
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }
    void Start()
    {
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnload;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (GameBrain.main == null || scene.name == GameBrain.main.gameScene)
        {
            if (!newSave) LoadData();
        }
    }
    private void OnSceneUnload(Scene scene)
    {
    }
    public void LoadData()
    {
        FindListeners();
        if (listeners != null)
        {
            foreach (var listener in listeners)
            {
                listener.LoadData(gameData);
            }
        }
    }
    public void SaveData()
    {
        FindListeners();
        if (listeners != null)
        {
            foreach (var listener in listeners)
            {
                listener.SaveData(ref gameData);
            }
        }
    }
    public void ResetData()
    {
        FindListeners();
        FileManager.DeleteFile(fileName);
        gameData = new GameData();
        newSave = true;
        GameBrain.main.ExitToMenu(false);

        if (listeners != null)
        {
            foreach (var listener in listeners)
            {
                listener.ResetData();
            }
        }
    }

    public bool ReadData()
    {
        gameData = FileManager.ReadFromFile<GameData>(fileName);
        if (gameData == null)
        {
            Debug.Log("created new save data");
            gameData = new GameData();
            return true;
        }
        return false;
    }
    public void WriteData()
    {
        FileManager.WriteToFile(fileName, gameData);
        newSave = false;
    }

    public void FindListeners()
    {
        
        listeners = FindObjectsOfType<MonoBehaviour>().OfType<ISaveData>().ToList();
        for (int i = 0; i < listeners.Count; i++)
            Debug.Log("listeners found: " + listeners[i].GetType().Name);
    }
}
