
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GlobalData : MonoBehaviour, ISaveData
{
    [SerializeField] private Transform teleportLocation;
    public static GlobalData instance;
    public bool isWalletUnlocked = false;
    private int UIHintFlags = 3;
    public MainScriptState storyProgress;


    public event Action OnUIHintFlagsChanged;
    public UnityEvent<MainScriptState> OnStoryChange;
    public enum MainScriptState
    {
        START = 0,
        START_MONOLOGUE = 1,
        CHURCH = 10,
        MARKET = 20,
        CAFE = 30,
        OLD_HOME = 40,
        NURSERY = 50,
    }

    public enum EHintBit
    {
        CONTROLS = 0x1,
        WALLET = 0x2,
    }

    public void UpdateStory(MainScriptState newState)
    {
        OnStoryChange?.Invoke(newState);
    }
    public void LoadData(GameData data)
    {
        storyProgress = (MainScriptState)data.storyProgress;
        UIHintFlags = data.UIflags;
    }
    public void SaveData(ref GameData data)
    {
        data.storyProgress = (int)storyProgress;
        data.UIflags = UIHintFlags;
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
        if (DialogueManager.instance != null)       
            DialogueManager.instance.OnDialogueEnd.AddListener(DialogueEnded);
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

    public void DialogueEnded(string name)
    {
        Debug.Log($"Dialogue '{name}' has ended.");

        switch(name)
        {
            case "OldHouseDialogue":
                UpdateStory(MainScriptState.OLD_HOME);
                TeleportPlayer();
                break;
        }
    }

    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if(player != null)
        {
            player.transform.position = teleportLocation.position;
        } else
        {
            Debug.LogWarning("Player object not found. Cannot teleport.");
        }
    }
}
