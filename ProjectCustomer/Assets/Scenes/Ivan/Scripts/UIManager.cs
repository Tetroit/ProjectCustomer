using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject controlsHint;
    public GameObject walletHint;

    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        GlobalData.instance.OnUIHintFlagsChanged.AddListener(UpdateHintsStatus);
        UpdateHintsStatus();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHintsStatus()
    {
        SetHintStatus(controlsHint, GlobalData.instance.GetUIHintStatus(GlobalData.EHintBit.CONTROLS));
        SetHintStatus(walletHint, GlobalData.instance.GetUIHintStatus(GlobalData.EHintBit.WALLET));
    }

    public void SetHintStatus(GameObject hint, bool active)
    {
        hint.SetActive(active);
    }
}
