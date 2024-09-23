using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI dialogueText;

    private string dialogueName;
    bool isSecondDialogue;
    private Queue<DialogueLine> dialogueLines;

    [SerializeField] Animator animator;

    [SerializeField] float textSpeed;

    public bool isDialogueFinished = false;

    [SerializeField] public UnityEvent<string> OnDialogueEnd;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }
    void Start()
    {
        dialogueLines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueName = dialogue.dialogueName;

        isDialogueFinished = false;
        OpenDialogue();
        dialogueLines.Clear();

        foreach(DialogueLine line in dialogue.firstDialogue)
        {
            dialogueLines.Enqueue(line);
        }

        DisplayNextSentence();
        isSecondDialogue = false;
    }

    public void StartSecondDialogue(Dialogue dialogue)
    {
        dialogueName = dialogue.dialogueName;

        isDialogueFinished = false;
        OpenDialogue();
        dialogueLines.Clear();

        foreach(DialogueLine line in dialogue.secondDialogue)
        {
            dialogueLines.Enqueue(line);
        }

        DisplayNextSentence();
        isSecondDialogue = true;
    }

    public void DisplayNextSentence()
    {
        if(dialogueLines.Count == 0)
        {
            isDialogueFinished = true;
            EndDialogue();

            if (!isSecondDialogue)
                OnDialogueEnd?.Invoke(dialogueName);

            return;
        }

        DialogueLine dialogueLine = dialogueLines.Dequeue();
        nameText.text = dialogueLine.name;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogueLine.sentence));
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
