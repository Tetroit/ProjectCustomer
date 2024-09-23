using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerControlsIvan pc = other.GetComponent<PlayerControlsIvan>();
        if (pc)
        {
            pc.isSafe = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlayerControlsIvan pc = other.GetComponent<PlayerControlsIvan>();
        if (pc)
        {
            pc.isSafe = true;
        }
    }
}
