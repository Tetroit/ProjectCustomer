using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractTrigger : MonoBehaviour
{
    static InteractTrigger active;
    InteractTrigger previous;

    public UnityEvent OnInteract;
    public Transform player;
    public Bounds interactableArea = new Bounds(new Vector3(0,0,0), new Vector3(1,1,1));
    Vector3 boundsOffset;
    public KeyCode interactKey = KeyCode.E;

    float distanceToObject;
    float proximity;

    bool isInteractable  = false;
    public float interactionDistance;
    public float maxAngle = 30;
    void Start()
    {
        boundsOffset = interactableArea.center;
    }

    // Update is called once per frame
    void Update()
    {
        previous = null;
        interactableArea.center = boundsOffset + transform.position;

        Ray screenRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        distanceToObject = Mathf.Sqrt(interactableArea.SqrDistance(player.position));
        proximity = Vector3.Dot(screenRay.direction, (interactableArea.center - player.position).normalized);

        if (distanceToObject < interactionDistance && proximity > Mathf.Cos(maxAngle * Mathf.Deg2Rad))
        {
            if (active == null)
            {
                    SetActive();
            }
            else
            {
                previous = active;
                if (active.distanceToObject / active.proximity > distanceToObject/proximity)
                {
                    SetActive();
                }
            }
        }
        else if (active == this)
        {
            if (previous == null)
                ClearActive();
            else
                previous.SetActive();
        }

        if (isInteractable && Input.GetKeyDown(interactKey))
        {
            OnInteract?.Invoke();
        }
    }

    public void YOUAREGAY()
    {
        Debug.Log("you talked to " + name + ", u feel stoobid");
    }
    void SetActive()
    {
        if (active == null)
        {
            active = this;
            isInteractable = true;
        }
        else
        {
            active.isInteractable = false;
            active = this;
            isInteractable = true;
        }
    }
    static void ClearActive()
    {
        active.isInteractable = false;
        active = null;
    }

    private void OnDrawGizmos()
    {
        if (isInteractable)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.blue;

        if (!Application.isPlaying)
            Gizmos.DrawWireCube(interactableArea.center + transform.position, interactableArea.size);
        else
            Gizmos.DrawWireCube(interactableArea.center, interactableArea.size);
    }
}
