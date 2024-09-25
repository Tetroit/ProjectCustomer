using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void OnDestroy()
    {
        Debug.Log("uihint destroyed");
        if (GlobalData.instance != null && GlobalData.instance.hints.Contains(this))
            GlobalData.instance.hints.Remove(this);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
