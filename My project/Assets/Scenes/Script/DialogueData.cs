using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DialogueLine
{
    public string name;      // 角色名字
    [TextArea(3, 10)]
    public string content;   // 对话内容
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Data")]
public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines;
}



