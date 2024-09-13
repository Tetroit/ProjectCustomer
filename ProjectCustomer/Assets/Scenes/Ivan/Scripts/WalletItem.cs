using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WalletItem : MonoBehaviour
{
    public BoxCollider hoverZone;
    public Transform item;
    public bool flipped;
    public Vector3 showingOffset = new Vector3(0, 0.5f, 0);
    Wallet wallet;
    void Start()
    {
        if (hoverZone == null)
            hoverZone = GetComponent<BoxCollider>();

        item = transform.GetChild(0);
        wallet = GetComponentInParent<Wallet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameBrain.main.gameState == GameState.INVENTORY)
        {
            Ray mouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (hoverZone.Raycast(mouse, out hit, Camera.main.farClipPlane))
            {
                if (Input.GetKeyDown(KeyCode.F))
                    flipped = !flipped;
                item.localPosition = Vector3.Lerp(item.localPosition, showingOffset, 0.02f);
            }
            else
                item.localPosition = Vector3.Lerp(item.localPosition, Vector3.zero, 0.02f);
            
            if (flipped)
                item.localEulerAngles = Vector3.Lerp(item.localEulerAngles, new Vector3(0, 180, 0), 0.03f);
            else
                item.localEulerAngles = Vector3.Lerp(item.localEulerAngles, new Vector3(0, 0, 0), 0.03f);
        }
    }

    bool HitTest(Ray ray)
    {
        RaycastHit hit;
        return (hoverZone.Raycast(ray, out hit, Camera.main.farClipPlane));
    }
}
