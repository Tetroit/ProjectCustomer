
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GlobalData : MonoBehaviour, ISaveData
{
    [SerializeField] private Transform teleportLocationPark;
    [SerializeField] private Transform teleportLocationNursery;
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
        CHURCH_CANDLE_LIT = 11,
        CHURCH_DIALOGUE = 12,
        MARKET = 20,
        CAFE = 30,
        CAFE_WALLET_PULLED = 31,
        CAFE_FINISHED = 32,
        OLD_HOME = 40,
        OLD_HOME_DIALOGUE = 41,
        NURSERY = 50,
        END = 60
    }

    public enum EHintBit
    {
        CONTROLS = 0x1,
        WALLET = 0x2,
    }

    public void UpdateStory(MainScriptState newState)
    {
        OnStoryChange?.Invoke(newState);
        storyProgress = newState;
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
            case "InnerDialogue":
                UpdateStory(MainScriptState.START_MONOLOGUE);
                break;
            case "CandleDialogue":
                UpdateStory(MainScriptState.CHURCH_DIALOGUE);
                break;
            case "CafeDirectionsDialogue":
                UpdateStory(MainScriptState.MARKET);
                break;
            case "BaristaDialogue":
                UpdateStory(MainScriptState.CAFE);
                break;
            case "ApproachOldHouseDialogue":
                UpdateStory(MainScriptState.OLD_HOME);
                break;
            case "OldHouseDialogue":
                UpdateStory(MainScriptState.OLD_HOME_DIALOGUE);
                TeleportPlayer(teleportLocationPark);
                break;
            case "NurseDialogue":
                UpdateStory(MainScriptState.NURSERY);
                TeleportPlayer(teleportLocationNursery);
                break;
        }
    }

    private void TeleportPlayer(Transform location)
    {
        GameObject player = GameObject.FindWithTag("Player");

        if(player != null)
        {
            player.transform.position = location.position;
        } else
        {
            Debug.LogWarning("Player object not found. Cannot teleport.");
        }
    }
}
