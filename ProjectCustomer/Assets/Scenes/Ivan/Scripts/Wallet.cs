using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    bool isOpen = false;
    public float rotationFac = 0.01f;
    public Vector3 offPosition = new Vector3 (-0.5f, -0.5f, 0.5f);
    public Vector3 onPosition = new Vector3 (0f,0f,2f);
    Vector3 targetPos;
    void Start()
    {
        targetPos = offPosition;
        transform.localPosition = offPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) )
        {
            Debug.Log("toggle");
            if (isOpen && GameBrain.main.gameState == GameState.INVENTORY)
                Close();
            else if (GameBrain.main.gameState == GameState.GAME)
                Open();
        }   

        if (GameBrain.main.gameState == GameState.INVENTORY)
        {

            Vector2 fac = (Input.mousePosition / new Vector2(Screen.width * 0.5f, Screen.height * 0.5f) - Vector2.one) * rotationFac;
            transform.localRotation = Quaternion.Euler(-fac.y, fac.x, 0);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.02f);
    }

    void Open()
    {
        targetPos = onPosition;
        isOpen = true;
        Debug.Log("open");
        GameBrain.main.ChangeGameState(GameState.INVENTORY);
    }
    void Close()
    {
        targetPos = offPosition;
        isOpen = false;
        Debug.Log("close");
        GameBrain.main.ChangeGameState(GameState.GAME);

    }
}
