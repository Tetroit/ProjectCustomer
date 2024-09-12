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

    [SerializeField] string tagFilter;

    [SerializeField] bool destroyOnTrigger;

    public event Action OnTriggeredEnter;
    public event Action OnTriggeredExit;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 triggerToPlayer = other.transform.position - transform.position;

        float dotProduct = Vector3.Dot(transform.forward, triggerToPlayer.normalized);

        if(dotProduct < 0.5f)
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

        // The trigger is destroyed when you exit it, so its activated only once
        //DestroyTrigger();
    }

    private void CheckTagsEnter(Collider other)
    {
        if(!string.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter))
        {
            onTriggerEnter.Invoke();
        }
    }

    private void CheckTagsExit(Collider other)
    {
        if(!string.IsNullOrEmpty(tagFilter) && !other.gameObject.CompareTag(tagFilter))
        {
            onTriggerExit.Invoke();
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
