using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LameFix : MonoBehaviour
{
    public Transform player;
    public DialogueManager dialogueManager;
    public DialogueTrigger dialogueTrigger;

    private bool isStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GlobalData.instance != null && Vector3.Distance(transform.position, player.position) < 3f && dialogueTrigger.isActive)
        {
            GlobalData.instance.UseHint(GlobalData.EUIHint.E);
        }

        if(Vector3.Distance(transform.position, player.position) < 3f && Input.GetKeyDown(KeyCode.E))
        {
            if(dialogueTrigger.isActive && dialogueTrigger != null)
            {
                if(!isStarted && dialogueTrigger != null && dialogueTrigger.isActive)
                {
                    dialogueTrigger.TriggerDialogue();
                    isStarted = true;
                }
            }

        }
    }
}
