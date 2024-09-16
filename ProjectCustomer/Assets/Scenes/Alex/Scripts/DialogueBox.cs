using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics.CodeAnalysis;

public class Dialogue : MonoBehaviour
{
    public aiControlls aiControlls;
    public TextMeshProUGUI textComponent;
    public string[] firstLines;
    public string[] lastLines;
    public float textSpeed;
    private int index;

    private bool interractedOnce = false;
    private bool inDialogue = false;
    
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(textComponent.text == GetCurrentLines()[index])
            {
                NextLines();
            } else
            {
                StopAllCoroutines();
                textComponent.text = GetCurrentLines()[index];
            }
        }
    }

    public void StartDialogue()
    {
        if(!inDialogue)
        {
            gameObject.SetActive(true);
            textComponent.text = string.Empty;
            StopAllCoroutines();
            index = 0;
            StartCoroutine(TypeLine());
            inDialogue = true;
        }
    }

    IEnumerator TypeLine()
    {
        foreach (char c in GetCurrentLines()[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLines()
    {
        if(index < GetCurrentLines().Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        gameObject.SetActive(false);
        aiControlls.ResumeIdle();
        inDialogue = false;
        interractedOnce = true;
    }

    private string[] GetCurrentLines()
    {
        return interractedOnce ? lastLines : firstLines;
    }

    public void ResetDialogue()
    {
        gameObject.SetActive(false);
        inDialogue = false;
        index = 0;
    }
}
