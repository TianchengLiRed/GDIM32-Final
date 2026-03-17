using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : Interactable
{

    [SerializeField] private GameObject NotePanel;

    private void Start()
    {
        NotePanel.SetActive(false);
    }

    // 重写父类的方法
    public override void OnInteract()
    {
        // 先保留父类的日志（可选）
        base.OnInteract();
        NotePanel.SetActive(true);

    }

    public void CloseNotePanel()
    {
        NotePanel.SetActive(false);
    }
}
