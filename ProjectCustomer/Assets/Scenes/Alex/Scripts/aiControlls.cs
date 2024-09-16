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

    public enum Enpc
    {
        Alex,
        Ivan,
        Jenifer
    }

    private EState currentState = EState.Idle;
    [SerializeField] Enpc enpc;

    public NavMeshAgent agent;
    public Transform player;
    public Dialogue dialoguePanel;

    public Vector3[] locations;
    private int currentLocationIndex = 0;

    private void Start()
    {

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

        if(Vector3.Distance(transform.position, player.position) > 4f && currentState != EState.Idle)
        {
            ResumeIdle();
            dialoguePanel.ResetDialogue();
        }
    }

    private void MoveToNextLocation()
    {
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
        agent.isStopped = true;
        Vector3 aiToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(aiToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        dialoguePanel.StartDialogue();
    }

    public void ResumeIdle()
    {
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
