using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
 public static DialogueManager Instance;

    public event Action<DialogueLine> OnLineStarted; // 当新的一行话开始
    public event Action OnDialogueEnded;            // 当对话结束

    private Queue<DialogueLine> _lineQueue = new Queue<DialogueLine>();
    private Action _onDialogueCompleted;
    public bool IsInDialogue { get; private set; }

    void Awake() => Instance = this;

   //对话开始
    public void StartDialogue(DialogueData data, Action onCompleted = null)
    {
        if (data == null) return;

        if (IsInDialogue) return;

        _lineQueue.Clear();
        foreach (var line in data.lines) _lineQueue.Enqueue(line);
        _onDialogueCompleted = onCompleted;

        IsInDialogue = true;
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (_lineQueue.Count == 0)//当没有对话时END
        {
            EndDialogue();
            return;
        }

        DialogueLine nextLine = _lineQueue.Dequeue();
        OnLineStarted?.Invoke(nextLine); //事件start通知
    }

    private void EndDialogue()
    {
        IsInDialogue = false;
        Action completed = _onDialogueCompleted;
        _onDialogueCompleted = null;
        OnDialogueEnded?.Invoke();//事件end通知
        Debug.Log("对话结束");

        if (completed != null)
        {
            StartCoroutine(InvokeCompletedNextFrame(completed));
        }
    }

    private IEnumerator InvokeCompletedNextFrame(Action completed)
    {
        yield return null;
        completed?.Invoke();
    }
}
