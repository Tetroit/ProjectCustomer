using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;
    public DialogueManager dialogueManager;
    public DialogueTrigger dialogueTrigger;

    [SerializeField] string tagFilterName;

    [SerializeField] bool destroyOnTrigger;
    public bool isStarted = false;
    private bool isStartedInnerMonologue = false;
    private bool isStartedOldHouseInnerDialogue = false;
    private bool isStartedParkInnerMonologue = false;

    private void OnTriggerEnter(Collider other)
    {
        //Compares whether objects with the assigned tag passes through the trigger
        CheckTagsEnter(other);

        //OldHouseInnerMonologue();

        // The trigger is destroyed when you enter it, so its activated only once
        DestroyTrigger();
    }

    private void OnTriggerExit(Collider other)
    {
        //Compares whether objects with the assigned tag exits the trigger
        CheckTagsExit(other);
        // trigger the dialogue
        //StartInnerMonologue();
        // The trigger is destroyed when you exit it, so its activated only once
        DestroyTrigger();
    }

    private void CheckTagsEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilterName) && other.gameObject.CompareTag(tagFilterName))
        {
            onTriggerEnter?.Invoke();
        }
    }

    private void CheckTagsExit(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilterName) && other.gameObject.CompareTag(tagFilterName))
        {
            onTriggerExit?.Invoke();
        }
    }

    public void TriggerDialogue()
    {
         dialogueTrigger.TriggerDialogue();
    }

    private void DestroyTrigger()
    {
        if (destroyOnTrigger)
        {
            Destroy(gameObject);
        }
    }
}
