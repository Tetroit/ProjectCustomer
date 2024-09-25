using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDestinationHint : MonoBehaviour
{
    public List<Sprite> destinations;
    public Image icon;
    int currentDestination = 0;
    void Start()
    {
        icon = GetComponent<Image>();
        icon.enabled = false;
        if (GlobalData.instance != null)
        {
            GlobalData.instance.OnStoryChange.AddListener(ChangeDestination);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeDestination(GlobalData.MainScriptState state)
    {
        switch (state)
        {
            case GlobalData.MainScriptState.START_MONOLOGUE:
                {
                    icon.enabled = true;
                    currentDestination = 0;
                    icon.sprite = destinations[currentDestination];
                    break;
                }
            case GlobalData.MainScriptState.CHURCH_DIALOGUE:
            case GlobalData.MainScriptState.MARKET:
            case GlobalData.MainScriptState.CAFE:
            case GlobalData.MainScriptState.OLD_HOME_DIALOGUE:
            case GlobalData.MainScriptState.PARK:
                {
                    icon.sprite = destinations[++currentDestination];
                    break;
                }
        }
        Debug.Log("NBULLSHIT" + currentDestination);
    }
}
