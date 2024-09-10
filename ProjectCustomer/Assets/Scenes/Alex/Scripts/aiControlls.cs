using System.Collections;
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

    public EState currentState = EState.Idle;

    public NavMeshAgent agent;

    public Transform player;

    public Vector3[] locations;
    private int currentLocationIndex = 0;

    // Update is called once per frame
    void Update()
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
            Debug.Log("Talk");
            currentState = EState.Talk;
        }

        if(Vector3.Distance(transform.position, player.position) > 4f && currentState != EState.Idle)
        {
            ResumeIdle();
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

        //Here we can add dialogue or smth
    }

    private void ResumeIdle()
    {
        agent.isStopped = false;
        currentState = EState.Idle;

        //I am not calling the MoveToNextLocation method because it was skipping a location
        if(locations.Length > 0)
        {
            agent.SetDestination(locations[currentLocationIndex - 1]);
            currentLocationIndex = (currentLocationIndex + 1) % locations.Length;
        }
    }
}
