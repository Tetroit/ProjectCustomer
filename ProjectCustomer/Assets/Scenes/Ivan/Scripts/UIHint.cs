using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHint : MonoBehaviour
{
    public int references = 0;
    public GlobalData.EUIHint hint;
    public bool isHidden;
    public void Show()
    {
        gameObject.SetActive(true);
        isHidden = false;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        isHidden = true;
    }
    void Start()
    {
        if (GlobalData.instance!= null)
        {
            GlobalData.instance.hints.Add(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
