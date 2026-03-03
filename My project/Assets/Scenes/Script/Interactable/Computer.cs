using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer: Interactable
{
     public override void OnInteract()
    {
        // 先保留父类的日志（可选）
        base.OnInteract();
        Debug.Log("computer");

        UIManager.Instance.OpenComputerPanel();
        AudioManager.Instance.PlayerComputer();

        // 比如：UIManager.Instance.ShowDialogue(noteContent);
    }
}
