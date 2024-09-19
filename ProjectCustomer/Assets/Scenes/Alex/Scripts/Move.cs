using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] float minDistance = 1f;
    [SerializeField] float maxDistance = 10f;

    public Trigger trigger;

    private NavMeshSurface navMeshSurface;

    // Start is called before the first frame update
    void Start()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>();

        if(trigger != null)
        {
            trigger.OnTriggeredEnter += OntriggerEntered;
            trigger.OnTriggeredExit += OntriggerExited;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OntriggerEntered()
    {
        MoveObject();
        RefreshNavMesh();
    }

    public void OntriggerExited()
    {
        //Implement this if you need some behavior on trigger exit
    }

    private void MoveObject()
    {
        Vector3 direction = new Vector3(0, 0, 1);
        float randomDistance = Random.Range(minDistance, maxDistance);
        Vector3 newPosition = transform.position + direction * randomDistance;
        transform.position = newPosition;
        Debug.Log("Position changed");
    }

    private void OnDestroy()
    {
        if(trigger != null)
        {
            trigger.OnTriggeredEnter -= OntriggerEntered;
            trigger.OnTriggeredExit -= OntriggerExited;
        }
    }

    private void RefreshNavMesh()
    {
        if(navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh(); // Rebuild the NavMesh
            Debug.Log("NavMesh baked");
        }
    }
}
