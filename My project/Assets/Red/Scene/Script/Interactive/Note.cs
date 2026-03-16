using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : Interactable
{
    [TextArea] public string noteContent = "这是一封神秘的信件...";

    // 重写父类的方法
    public override void OnInteract()
    {
        // 先保留父类的日志（可选）
        base.OnInteract();

        // 这里写你自己的逻辑，比如弹出 UI 对话框
        Debug.Log("读取纸条内容：" + noteContent);

        // 比如：UIManager.Instance.ShowDialogue(noteContent);
    }
}
