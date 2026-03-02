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
    private int _dialogueStartedFrame = -1;
    private int _lastAdvanceFrame = -1;
    private int _lastDialogueEndedFrame = -1000;
    private DialogueData _currentDialogueData;
    private DialogueData _lastDialogueData;
    public bool IsInDialogue { get; private set; }
    public bool CanAdvanceDialogue => IsInDialogue && Time.frameCount > _dialogueStartedFrame;

    void Awake() => Instance = this;

   //对话开始
    public void StartDialogue(DialogueData data, Action onCompleted = null)
    {
        if (data == null) return;

        if (IsInDialogue) return;
        if (_lastDialogueData == data && Time.frameCount <= _lastDialogueEndedFrame + 1) return;

        _lineQueue.Clear();
        foreach (var line in data.lines) _lineQueue.Enqueue(line);
        _onDialogueCompleted = onCompleted;
        _currentDialogueData = data;
        _dialogueStartedFrame = Time.frameCount;
        _lastAdvanceFrame = -1;

        IsInDialogue = true;
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (!IsInDialogue) return;
        if (_lastAdvanceFrame == Time.frameCount) return;

        _lastAdvanceFrame = Time.frameCount;

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
        _dialogueStartedFrame = -1;
        _lastDialogueEndedFrame = Time.frameCount;
        _lastDialogueData = _currentDialogueData;
        _currentDialogueData = null;
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
