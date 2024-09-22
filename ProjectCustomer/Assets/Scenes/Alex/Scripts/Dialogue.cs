using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string name;

    public string sentence;
}

[System.Serializable]
public class Dialogue
{
    public DialogueLine[] firstDialogue;
    public DialogueLine[] secondDialogue;
}