using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueLine
{
    public string name;

    [TextArea(3, 10)]
    public string sentence;
}

[System.Serializable]
public class Dialogue
{
    [SerializeField]
    public string dialogueName = "dialogue";
    [SerializeField]
    public DialogueLine[] firstDialogue;
    [SerializeField]
    public DialogueLine[] secondDialogue;
}