using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aiControlls : MonoBehaviour
{
    public enum EState
    {
        Idle,
        Talk
    }

    

    private EState currentState = EState.Idle;
    [SerializeField] Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public DialogueManager dialogueManager;
    public DialogueTrigger DialogueTrigger;
    

    public Vector3[] locations;
    private int currentLocationIndex = 0;

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
        }

        if(Vector3.Distance(transform.position, player.position) < 4f && Input.GetKeyDown(KeyCode.E))
        {
            currentState = EState.Talk;
        }

        if(Vector3.Distance(transform.position, player.position) > 4f && currentState != EState.Idle || dialogueManager.isDialogueFinished)
        {
            ResumeIdle();
            dialogueManager.isDialogueFinished = false;
        }
    }

    private void MoveToNextLocation()
    {
        animator.SetBool("Walking", true);
        if(locations.Length > 0)
        {
            agent.SetDestination(locations[currentLocationIndex]);
            currentLocationIndex = (currentLocationIndex + 1) % locations.Length;
        }
    }

    private void HandleIdleState()
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

        if(!isStarted)
        {
            DialogueTrigger.TriggerDialogue();
            isStarted = true;
        }
        
        if(Input.GetKeyDown(KeyCode.F))
        {
            dialogueManager.DisplayNextSentence();
        }
    }

    public void ResumeIdle()
    {
        animator.SetBool("Walking", true);
        dialogueManager.CloseDialogue();
        isStarted = false;
        agent.isStopped = false;
        currentState = EState.Idle;

        if(locations.Length > 0)
        {
            int targetIndex = (currentLocationIndex - 1 + locations.Length) % locations.Length;
            agent.SetDestination(locations[targetIndex]);
            currentLocationIndex = (currentLocationIndex) % locations.Length;
        }
    }
}
