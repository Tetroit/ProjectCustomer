using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveData
{
    void SaveData(ref GameData data); 
    void LoadData(GameData data);

    void ResetData();
}
[System.Serializable]

public class GameData
{
    [SerializeField]
    public Vector3 playerPos = Vector3.zero;
    [SerializeField]
    public int storyProgress = 0;
    [SerializeField]
    public bool isWalletUnlocked = false;
    [SerializeField]
    public bool isCutscenePlayed = false;
}
