using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<Collider>() == null)
            Debug.LogAssertion("collider not attached");
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerControlsIvan pc = collision.gameObject.GetComponent<PlayerControlsIvan>();
            if (pc != null)
            {
                pc.transform.position = pc.lastSafePos;
            }
        }
    }
}
