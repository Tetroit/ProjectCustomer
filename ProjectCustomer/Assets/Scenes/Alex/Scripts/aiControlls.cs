using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class aiControlls : MonoBehaviour
{
    public enum EState
    {
        Idle,
        Walk,
        Talk
    }



    private EState currentState = EState.Idle;
    [SerializeField] Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public DialogueManager dialogueManager;
    public DialogueTrigger dialogueTrigger;


    public Vector3[] locations;
    private int currentLocationIndex = 0;

    private Vector3 targetPosition = Vector3.zero;
    private float reachRange = 0.2f;

    private bool isStarted = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SwitchStates();
    }

    private void SwitchStates()
    {
        switch(currentState)
        {
            case EState.Idle:
                HandleIdleState();
                break;
            case EState.Talk:
                HandleTalkState();
                break;
            case EState.Walk:
                HandleWalkState();
                break;
        }

        if(Vector3.Distance(transform.position, player.position) < 4f && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogueTrigger.isActive && dialogueTrigger != null)
            {
                currentState = EState.Talk;
            }
                
        }

        if(Vector3.Distance(transform.position, player.position) > 4f && currentState != EState.Idle && currentState != EState.Walk && dialogueManager.isDialogueFinished)
        {
            if(locations.Length > 0)
            {
                ResumeWalking();
            } else
            {
                ResumeIdle();
            }

            dialogueManager.isDialogueFinished = false;
        }
    }

    private void SetDestination(Vector3 destination)
    {
        if (!NavMesh.SamplePosition(destination, out NavMeshHit hit, 1f, 1))
        {
            Debug.LogError($"Failed to find position");
            return;
        }

        targetPosition = hit.position;
        agent.SetDestination(targetPosition);
    }

    private void MoveToNextLocation()
    {
        animator.SetBool("Walking", true);
        if(locations.Length > 0)
        {
            SetDestination(locations[currentLocationIndex]);
            currentLocationIndex = (currentLocationIndex + 1) % locations.Length;
        }
    }

    private void HandleIdleState()
    {
        if(locations == null || locations.Length == 0)
        {
            //animator.SetBool("Walking", false);
            agent.isStopped = true;
        } else
        {
            agent.isStopped = false;
            currentState = EState.Walk;
        }
    }

    private void HandleWalkState()
    {
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextLocation();
        }
    }

    private void HandleTalkState()
    {
        animator.SetBool("Walking", false);
        agent.isStopped = true;
        Vector3 aiToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(aiToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if(!isStarted && dialogueTrigger != null && dialogueTrigger.isActive)
        {
            dialogueTrigger.TriggerDialogue();
            isStarted = true;
        }
    }

    public void ResumeWalking()
    {
        Debug.Log($"resume walking");

        currentState = EState.Walk;
        dialogueManager.CloseDialogue();
        animator.SetBool("Walking", true);
        agent.isStopped = false;
        isStarted = false;

        if(locations.Length > 0)
        {
            int targetIndex = (currentLocationIndex - 1 + locations.Length) % locations.Length;
            agent.SetDestination(locations[targetIndex]);
            currentLocationIndex = (currentLocationIndex) % locations.Length;
        }
    }

    public void ResumeIdle()
    {
        animator.SetBool("Walking", false);
        dialogueManager.CloseDialogue();
        isStarted = false;
        agent.isStopped = true;
        currentState = EState.Idle;
    }
}
