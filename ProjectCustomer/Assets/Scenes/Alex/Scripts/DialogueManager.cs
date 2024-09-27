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

    public string dialogueName;
    bool isSecondDialogue;
    public Queue<DialogueLine> dialogueLines;

    [SerializeField] Animator animator;

    [SerializeField] float textSpeed;

    public bool isDialogueFinished = false;

    [SerializeField] public UnityEvent<string> OnDialogueEnd;

    private void Awake()
    {
        if (instance == null)
        {
            //DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
            Destroy(gameObject);
    }
    void Start()
    {
        dialogueLines = new Queue<DialogueLine>();
        if (GlobalData.instance != null)
            OnDialogueEnd.AddListener(GlobalData.instance.DialogueEnded);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("first");
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
        Debug.Log("second");
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
        Debug.Log("lines left: " + dialogueLines.Count);
        if(dialogueLines.Count == 0)
        {
            isDialogueFinished = true;
            EndDialogue();

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
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void EndDialogue()
    {
        CloseDialogue();
        if (!isSecondDialogue)
            OnDialogueEnd?.Invoke(dialogueName);
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
