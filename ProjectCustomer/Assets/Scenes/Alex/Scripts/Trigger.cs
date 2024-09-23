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
    private bool isStartedOldHouseInnerDialogue = false;

    private void OnTriggerEnter(Collider other)
    {
        OldHouseInnerMonologue();
        Debug.Log("Enter");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        StartInnerMonologue();
    }

    private void CheckTagsEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilterName) && other.gameObject.CompareTag(tagFilterName))
        {
            onTriggerEnter.Invoke();
        }
    }

    private void CheckTagsExit(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilterName) && other.gameObject.CompareTag(tagFilterName))
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
        if (!isStartedOldHouseInnerDialogue)
        {
            dialogueTrigger.TriggerDialogue();
            isStartedOldHouseInnerDialogue = true;
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
