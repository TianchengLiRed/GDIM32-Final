using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
   [SerializeField] private DialogueData myDialogue;

    public override void OnInteract()
    {
        // 触发交互时，告诉管理器开始这段对话
        if (myDialogue != null)
        {
            //调用scriptobject的对话信息
            DialogueManager.Instance.StartDialogue(myDialogue);
        }
    }
}
