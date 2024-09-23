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
    private bool isStartedInnerMonologue = false;
    private bool isStartedInnerMonologue2 = false;

    public event Action OnTriggeredEnter;
    public event Action OnTriggeredExit;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggeredEnter?.Invoke();
        //Compares whether objects with the assigned tag passes through the trigger
        Debug.Log("Enter");
        CheckTagsEnter(other);
        OldHouseInnerMonologue();
        GlobalData.instance.UpdateStory(GlobalData.MainScriptState.OLD_HOME);
        // The trigger is destroyed when you enter it, so its activated only once
        DestroyTrigger();
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggeredExit?.Invoke();
        //Compares whether objects with the assigned tag exits the trigger
        CheckTagsExit(other);
        // trigger the dialogue
        Debug.Log("Exit");
        StartInnerMonologue();
        GlobalData.instance.UpdateStory(GlobalData.MainScriptState.START);
        // The trigger is destroyed when you exit it, so its activated only once
        DestroyTrigger();
    }

    private void CheckTagsEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilterName) && !other.gameObject.CompareTag(tagFilterName))
        {
            onTriggerEnter.Invoke();
        }
    }

    private void CheckTagsExit(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilterName) && !other.gameObject.CompareTag(tagFilterName))
        {
            onTriggerExit.Invoke();
        }
    }

    public void StartInnerMonologue()
    {

        if (!isStartedInnerMonologue)
        {
            dialogueTrigger.TriggerDialogue();
            isStartedInnerMonologue = true;
        }
    }

    public void OldHouseInnerMonologue()
    {
        if (!isStartedInnerMonologue2)
        {
            dialogueTrigger.TriggerDialogue();
            isStartedInnerMonologue2 = true;
        }
    }

    private void DestroyTrigger()
    {
        if (destroyOnTrigger)
        {
            Destroy(gameObject);
        }
    }
}
