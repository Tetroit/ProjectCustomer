using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    public GameObject hoverImage; 
    public void OnHoverEnter()
    {
        Debug.Log("Hover Enter triggered");
        hoverImage.SetActive(true);  
    }

    public void OnHoverExit()
    {
        Debug.Log("Hover Exit triggered");
        hoverImage.SetActive(false);  
    }
}
