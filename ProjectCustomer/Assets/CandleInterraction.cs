using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static aiControlls;

public class CandleInterraction : MonoBehaviour
{
    public Transform player;
    public DialogueTrigger dialogueTrigger;

    bool isStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.position) < 4f && Input.GetKeyDown(KeyCode.E))
        {
            if(!isStarted)
            {
                dialogueTrigger.TriggerDialogue();
                isStarted = true;
            }
        }
    }
}
