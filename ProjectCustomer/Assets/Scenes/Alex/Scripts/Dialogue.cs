using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public DialogueLine[] firstDialogue;
    [SerializeField]
    public DialogueLine[] secondDialogue;
}