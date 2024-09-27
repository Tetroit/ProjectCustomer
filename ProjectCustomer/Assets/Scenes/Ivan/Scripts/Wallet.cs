using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wallet : MonoBehaviour
{
    bool isOpen = false;
    public float rotationFac = 0.01f;
    public Vector3 offPosition = new Vector3 (-0.25f, -0.25f, 0.25f);
    public Vector3 onPosition = new Vector3 (0f,0f,1f);
    Vector3 targetPos;
    public UnityEvent OnWalletPull;
    void Start()
    {
        targetPos = offPosition;
        transform.localPosition = offPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) )
        {
            if (GlobalData.instance.isWalletUnlocked)
            {
                if (isOpen && GameBrain.main.gameState == GameState.INVENTORY)
                    Close();
                else if (GameBrain.main.gameState == GameState.GAME)
                    Open();
            }
            else
            {
                Debug.Log("wallet not avaliable");
            }
        }   

        if (GameBrain.main.gameState == GameState.INVENTORY)
        {

            Vector2 fac = (Input.mousePosition / new Vector2(Screen.width * 0.5f, Screen.height * 0.5f) - Vector2.one) * rotationFac;
            transform.localRotation = Quaternion.Euler(-fac.y, fac.x, 0);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * 3f);
    }

    void Open()
    {
        targetPos = onPosition;
        isOpen = true;
        GameBrain.main.ChangeGameState(GameState.INVENTORY);

        if (GlobalData.instance != null && GlobalData.instance.storyProgress == GlobalData.MainScriptState.CAFE)
            GlobalData.instance.UpdateStory(GlobalData.MainScriptState.CAFE_WALLET_PULLED);
        OnWalletPull?.Invoke();
    }
    void Close()
    {
        targetPos = offPosition;
        isOpen = false;
        GameBrain.main.ChangeGameState(GameState.GAME);

    }
}
