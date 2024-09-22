using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;

    private Queue<DialogueLine> dialogueLines;

    [SerializeField] Animator animator;

    [SerializeField] float textSpeed;

    public bool isDialogueFinished = false;

    void Start()
    {
        dialogueLines = new Queue<DialogueLine>();
    }

    void Update()
    {

    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueFinished = false;
        OpenDialogue();
        dialogueLines.Clear();

        foreach(DialogueLine line in dialogue.firstDialogue)
        {
            dialogueLines.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void StartSecondDialogue(Dialogue dialogue)
    {
        isDialogueFinished = false;
        OpenDialogue();
        dialogueLines.Clear();

        foreach(DialogueLine line in dialogue.secondDialogue)
        {
            dialogueLines.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(dialogueLines.Count == 0)
        {
            isDialogueFinished = true;
            EndDialogue();
            return;
        }

        DialogueLine dialogLine = dialogueLines.Dequeue();
        nameText.text = dialogLine.name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogLine.sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    

    private void EndDialogue()
    {
        CloseDialogue();
    }
    public void OpenDialogue()
    {
        animator.SetBool("IsOpen", true);
    }

    public void CloseDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}
