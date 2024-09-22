using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private bool hasPlayedFirstDialogue = false;
    public void TriggerDialogue()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();

        if(!hasPlayedFirstDialogue)
        {
            dialogueManager.StartDialogue(dialogue);
            hasPlayedFirstDialogue = true;

        } else
        {
            dialogueManager.StartSecondDialogue(dialogue);
        }
    }
}