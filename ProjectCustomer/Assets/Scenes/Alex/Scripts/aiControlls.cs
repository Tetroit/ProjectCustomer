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

    public Vector3[] locations;
    private int currentLocationIndex = 0;

    private bool isTriggeredOnce = false;
    private bool isInterracted = false;

    // Dialogue management
    private List<string> alexDialogue = new List<string>();
    private int dialogueIndex = 0;

    private void Start()
    {
        // Initialize Alex's dialogue lines
        alexDialogue.Add("Player: Young man/lady, could you help me please. I need to go home… ");
        alexDialogue.Add("NPC: I suppose… what do you want? ");
        alexDialogue.Add("Player: I need to find… the… I need to… what was it called again?");
        alexDialogue.Add("NPC: Can you speak a bit faster? I don’t have all day.");
        alexDialogue.Add("Player: I want to go to… uhmm…");
        alexDialogue.Add("NPC: I don’t have time for this.  *leaves*");
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
            isInterracted = true;
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

        if(!isTriggeredOnce)
        {
            TriggerDialogueForNPC();
            isTriggeredOnce = true;
        }

        if(enpc == Enpc.Alex && Input.GetKeyDown(KeyCode.F))
        {
            ShowNextDialogueLine();
        }
    }

    private void TriggerDialogueForNPC()
    {
        switch(enpc)
        {
            case Enpc.Alex:
                dialogueIndex = 0;
                Debug.Log(alexDialogue[dialogueIndex]);
                break;

            case Enpc.Ivan:
                Debug.Log("This is the unique dialogue for Ivan.");
                break;

            case Enpc.Jenifer:
                Debug.Log("This is the unique dialogue for Jenifer.");
                break;

            default:
                Debug.Log("No specific dialogue for this NPC.");
                break;
        }
    }

    private void ShowNextDialogueLine()
    {
        dialogueIndex++;
        if(dialogueIndex < alexDialogue.Count)
        {
            Debug.Log(alexDialogue[dialogueIndex]);
        } else
        {
            ResumeIdle();
        }
    }

    private void ResumeIdle()
    {
        agent.isStopped = false;
        currentState = EState.Idle;

        // I am not calling the MoveToNextLocation method because it was skipping a location
        if(locations.Length > 0)
        {
            agent.SetDestination(locations[currentLocationIndex - 1]);
            currentLocationIndex = (currentLocationIndex) % locations.Length;
        }
    }
}
