using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class aiControlls : MonoBehaviour
{
    public NavMeshAgent agent;

    public Vector3[] locations;
    private int currentLocationIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextLocation();
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
}
