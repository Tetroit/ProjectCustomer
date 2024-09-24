
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class GlobalData : MonoBehaviour, ISaveData
{
    [SerializeField] private Transform teleportLocationPark;
    [SerializeField] private Transform teleportLocationNursery;
    public CameraFade cameraFade;
    public static GlobalData instance;
    public bool isWalletUnlocked = false;
    public MainScriptState storyProgress;

    public List<UIHint> hints;

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
        PARK = 50,
        NURSERY = 60,
        END = 70
    }

    public enum EUIHint
    {
        NULL = 0,
        E = 1,
        R = 2,
        F = 3,
        TAB = 4,
    }

    public void UpdateStory(MainScriptState newState)
    {
        OnStoryChange?.Invoke(newState);
        storyProgress = newState;
    }
    public void LoadData(GameData data)
    {
        storyProgress = (MainScriptState)data.storyProgress;
    }
    public void SaveData(ref GameData data)
    {
        data.storyProgress = (int)storyProgress;
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
        foreach (UIHint hint in hints)
            hint.Hide();

    }
    void Update()
    {
        if (isWalletUnlocked)
            UseHint(EUIHint.R);
        UpdateHints();
    }

    public void UseHint(EUIHint hint)
    {
        if (hint == EUIHint.NULL) return;
        foreach (UIHint UIHint in hints)
        {
            if (UIHint.hint == hint) UIHint.references++;
        }
    }
    void UpdateHints()
    {
        foreach (UIHint hint in hints)
        {
            if (hint.references > 0)
            {
                if (hint.isHidden)
                    hint.Show();
            }
            else
            {
                if (!hint.isHidden)
                    hint.Hide();
            }

            hint.references = 0;
        }
    }
    public static void UnlockWallet()
    {
        instance.isWalletUnlocked = true;
    }
    public void SetUIHintVisibility(int hintFlags, bool visibility) 
    {
    }

    public void DialogueEnded(string name)
    {
        Debug.Log($"Dialogue '{name}' has ended.");

        switch(name)
        {
            case "InnerDialogue":
                UpdateStory(MainScriptState.START_MONOLOGUE);
                
                break;

            case "EnterChurchDialogue":
                UpdateStory(MainScriptState.CHURCH);
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
                cameraFade.FadeIn();
                TeleportPlayer(teleportLocationPark);
                cameraFade.FadeOut();
                break;

            case "ParkDialogue":
                UpdateStory(MainScriptState.PARK);
                break;

            case "NurseDialogue":
                UpdateStory(MainScriptState.NURSERY);
                cameraFade.FadeIn();
                TeleportPlayer(teleportLocationNursery);
                cameraFade.FadeOut();
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
