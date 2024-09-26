using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDestinationHint : MonoBehaviour
{
    public List<Sprite> destinations;
    public Image icon;
    int currentDestination = 0;
    private void Awake()
    {
        icon = GetComponent<Image>();
        icon.enabled = false;
        if (GlobalData.instance != null)
        {
            GlobalData.instance.OnStoryChange.AddListener(ChangeDestination);
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeDestination(GlobalData.MainScriptState state)
    {
        if ((int) state >= 10)
        {
            icon.enabled = true;
        }
        switch (state)
        {
            case GlobalData.MainScriptState.START_MONOLOGUE:
            case GlobalData.MainScriptState.CHURCH:
            case GlobalData.MainScriptState.CHURCH_CANDLE_LIT:
                {
                    icon.sprite = destinations[0];
                    break;
                }
            case GlobalData.MainScriptState.CHURCH_DIALOGUE:
                {
                    icon.sprite = destinations[1];
                    break;
                }
            case GlobalData.MainScriptState.MARKET:
            case GlobalData.MainScriptState.CAFE:
            case GlobalData.MainScriptState.CAFE_WALLET_PULLED:
                {
                    icon.sprite = destinations[2];
                    break;
                }
            case GlobalData.MainScriptState.CAFE_FINISHED:
            case GlobalData.MainScriptState.OLD_HOME:
                {
                    icon.sprite = destinations[3];
                    break;
                }
            case GlobalData.MainScriptState.OLD_HOME_DIALOGUE:
            case GlobalData.MainScriptState.PARK:
                {
                    icon.sprite = destinations[4];
                    break;
                }
            case GlobalData.MainScriptState.NURSERY:
                {
                    icon.sprite = destinations[5];
                    break;
                }
        }
        Debug.Log("NBULLSHIT" + currentDestination);
    }
}
