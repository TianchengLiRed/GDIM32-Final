using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct DialogueLine
{
    public string name;      // 角色名字
    [TextArea(3, 10)]
    public string content;   // 对话内容
    public AudioClip voice;  // 可选：这段话的语音
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines;
}
