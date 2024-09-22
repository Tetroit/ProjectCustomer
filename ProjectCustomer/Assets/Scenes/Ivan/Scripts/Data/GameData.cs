using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveData
{
    void SaveData(ref GameData data); 
    void LoadData(GameData data);
}

[System.Serializable]
public class GameData
{
    [SerializeField]
    public Vector3 playerPos = Vector3.zero;
    [SerializeField]
    public int UIflags = 0;
    [SerializeField]
    public int storyProgress = 0;
}
