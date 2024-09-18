using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData instance;
    public bool isWalletUnlocked = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    void Start()
    {
    }
    void Update()
    {
        
    }

    public void UnlockWallet()
    {
        isWalletUnlocked = true;
    }
}
