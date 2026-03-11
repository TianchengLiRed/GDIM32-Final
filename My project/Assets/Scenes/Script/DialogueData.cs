using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;      // 角色名字
    public Sprite portrait;
    public AudioClip voiceClip;
    [TextArea(3, 10)]
    public string content;   // 对话内容

    public List<string> replyOptions;
    public List<DialogueData> replyBranches;

    public bool loopQuestion;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines;
}



