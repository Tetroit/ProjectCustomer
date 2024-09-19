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
    private bool isStarted = false;
    private bool isDialogueRunning = false;

    public event Action OnTriggeredEnter;
    public event Action OnTriggeredExit;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(isDialogueRunning)
        {
            StartInnerMonologue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 triggerToPlayer = other.transform.position - transform.position;

        float dotProduct = Vector3.Dot(transform.forward, triggerToPlayer.normalized);

        if(dotProduct > 0.5f)
        {
            OnTriggeredEnter?.Invoke();
            //Compares whether objects with the assigned tag passes through the trigger
            CheckTagsEnter(other);

            // The trigger is destroyed when you enter it, so its activated only once
            DestroyTrigger();
        } else
        {
            Debug.Log("Trigger not activated because the player is not facing the trigger.");
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggeredExit?.Invoke();
        //Compares whether objects with the assigned tag exits the trigger
        CheckTagsExit(other);
        isDialogueRunning = true;
        // The trigger is destroyed when you exit it, so its activated only once
        DestroyTrigger();
    }

    private void CheckTagsEnter(Collider other)
    {
        if(!string.IsNullOrEmpty(tagFilterName) && !other.gameObject.CompareTag(tagFilterName))
        {
            onTriggerEnter.Invoke();
        }
    }

    private void CheckTagsExit(Collider other)
    {
        if(!string.IsNullOrEmpty(tagFilterName) && !other.gameObject.CompareTag(tagFilterName))
        {
            onTriggerExit.Invoke();
        }
    }

    public void StartInnerMonologue()
    {
        if(!isStarted)
        {
            dialogueTrigger.TriggerDialogue();
            isStarted = true;
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            dialogueManager.DisplayNextSentence();
        }
    }

    private void DestroyTrigger()
    {
        if(destroyOnTrigger)
        {
            Destroy(gameObject);
        }
    }
}
