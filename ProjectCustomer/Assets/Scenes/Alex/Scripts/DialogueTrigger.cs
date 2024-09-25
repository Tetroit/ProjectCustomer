using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;
    public bool isActive = true;
    private bool hasPlayedFirstDialogue = false;

    public GlobalData.MainScriptState activateState;

    public void Start()
    {
        if (GlobalData.instance != null)
        {
            GlobalData.instance.OnStoryChange.AddListener(ActivateDialogue);
            ActivateDialogue(GlobalData.instance.storyProgress);
        }
    }
    public void TriggerDialogue()
    {
        if (isActive)
        {
            DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

            if (dialogueManager != null)
            {
                if (!hasPlayedFirstDialogue)
                {
                    dialogueManager.StartDialogue(dialogue);
                    hasPlayedFirstDialogue = true;

                }
                else
                {
                    dialogueManager.StartSecondDialogue(dialogue);
                }
            }
        }
    }

    public void ActivateDialogue(GlobalData.MainScriptState state)
    {
        isActive = (state == activateState);
    }
}