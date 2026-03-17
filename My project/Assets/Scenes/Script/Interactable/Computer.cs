using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer: Interactable
{
    [SerializeField] private GameObject ComputerPanel;
    public void Start()
    {
        ComputerPanel.SetActive(false);
    }
     public override void OnInteract()
    {
        // 先保留父类的日志（可选）
        base.OnInteract();
        Debug.Log("computer");

        AudioManager.Instance.PlayerComputer();
        ComputerPanel.SetActive(true);
        DialogueManager.Instance.ShowCursor();
    }

    public void CloseComputerPanel()
    {
        ComputerPanel.SetActive(false);
        DialogueManager.Instance.HideCursor();
    }

    public void emailComplete()
    {
        TaskManager.Instance.CompleteTask(TaskType.FinishEmail);
        AudioManager.Instance.PlayerComputer();
    }

    public void formComplete()
    {
        TaskManager.Instance.CompleteTask(TaskType.FinishForm);
        AudioManager.Instance.PlayerComputer();
    }
}
