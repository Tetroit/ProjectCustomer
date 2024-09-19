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

    private void Awake()
    {
        if (instance == null)
        {
            ReadData();
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
        FindListeners();
        LoadData();
    }
    private void OnSceneUnload(Scene scene)
    {
    }
    public void LoadData()
    {
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
        if (listeners != null)
        {
            foreach (var listener in listeners)
            {
                listener.SaveData(ref gameData);
            }
        }
    }

    public void ReadData()
    {
        gameData = FileManager.ReadFromFile<GameData>(fileName);
        if (gameData == null)
        {
            Debug.Log("created new save data");
            gameData = new GameData();
        }
    }
    public void WriteData()
    {
        FileManager.WriteToFile(fileName, gameData);
    }

    public void FindListeners()
    {
        listeners = FindObjectsOfType<MonoBehaviour>().OfType<ISaveData>().ToList();
    }
}
