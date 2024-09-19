
using System;
using UnityEngine;
using UnityEngine.Events;

public class GlobalData : MonoBehaviour
{
    public static GlobalData instance;
    public bool isWalletUnlocked = false;
    public int UIHintFlags = 3;

    public UnityEvent OnUIHintFlagsChanged;

    public enum EHintBit
    {
        CONTROLS = 0x1,
        WALLET = 0x2,
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    void Start()
    {
    }
    void Update()
    {
    }

    public static void UnlockWallet()
    {
        instance.isWalletUnlocked = true;
    }
    public void SetUIHintVisibility(int hintFlags, bool visibility) 
    {
        if (visibility)
            UIHintFlags |= hintFlags;
        else
            UIHintFlags &= ~hintFlags;

        OnUIHintFlagsChanged?.Invoke();
    }
    public bool GetUIHintStatus(EHintBit bit)
    {
        return (UIHintFlags & (int)bit) != 0;
    }
}
