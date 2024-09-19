using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;

    private Queue<string> sentences;
    private Queue<Dialogue> dialogueQueue;

    [SerializeField] Animator animator;

    [SerializeField] float textSpeed;

    public bool isDialogueFinished = false;
    private bool isDialogueActive = false;

    void Start()
    {
        sentences = new Queue<string>();
        dialogueQueue = new Queue<Dialogue>();
    }

    void Update()
    {

    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueFinished = false;
        OpenDialogue();
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach(string sentence in dialogue.firstDialogue)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void StartSecondDialogue(Dialogue dialogue)
    {
        isDialogueFinished = false;
        OpenDialogue();
        nameText.text = dialogue.name;
        sentences.Clear();

        // Enqueue second dialogue sentences
        foreach(string sentence in dialogue.secondDialogue)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            isDialogueFinished = true;
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
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

    public void EndDialogue()
    {
        CloseDialogue();
    }

    public bool IsDialogueComplete()
    {
        
        return sentences.Count < 0 && !isDialogueActive;
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
