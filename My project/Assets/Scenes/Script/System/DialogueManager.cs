using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueManager : MonoBehaviour
{
 public static DialogueManager Instance;

    public event Action<DialogueLine> OnLineStarted; // 当新的一行话开始
    public event Action OnDialogueEnded;            // 当对话结束
    public event Action<DialogueLine> OnDialogueReplied;

    private Queue<DialogueLine> _lineQueue = new Queue<DialogueLine>();
    private DialogueLine _currentLine;
    private DialogueLine _returnLine;
    private bool _waitingLoopChoice;
    public bool IsWaitingLoopChoice => _waitingLoopChoice;
    public bool IsInDialogue { get; private set; }


    private Action _onDialogueCompleted;
    private int _dialogueStartedFrame = -1;
    private int _lastAdvanceFrame = -1;
    private int _lastDialogueEndedFrame = -1000;
    private DialogueData _currentDialogueData;
    private DialogueData _lastDialogueData;
    public bool CanAdvanceDialogue => IsInDialogue && Time.frameCount > _dialogueStartedFrame;

    void Awake() => Instance = this;

   //对话开始
    public void StartDialogue(DialogueData data, Action onCompleted = null)
    {
        Debug.Log("Dialogue Started");
        if (data == null) return;

        if (IsInDialogue) return;
        if (_lastDialogueData == data && Time.frameCount <= _lastDialogueEndedFrame + 1) return;

        _lineQueue.Clear();
        _currentLine = null;
        _returnLine = null;
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

    // 如果当前正停在 loop question 上，玩家这时候点 Next = 结束对话
    if (_waitingLoopChoice)
    {
        _returnLine = null;
        EndDialogue();
        return;
    }

    // 队列空了
    if (_lineQueue.Count == 0)
    {
        // 如果之前记住了 loop question，就回到那个问题
        if (_returnLine != null)
        {
            _currentLine = _returnLine;
            _waitingLoopChoice = true;

            OnLineStarted?.Invoke(_currentLine);
            OnDialogueReplied?.Invoke(_currentLine);
            return;
        }

        EndDialogue();
        return;
    }

    _currentLine = _lineQueue.Dequeue();
    OnLineStarted?.Invoke(_currentLine);

    // 只有“当前行真的有选项”时，才触发 reply
    if (_currentLine.replyOptions != null && _currentLine.replyOptions.Count > 0)
    {
        if (_currentLine.loopQuestion)
        {
            _returnLine = _currentLine;
            _waitingLoopChoice = true;
        }
        else
        {
            _waitingLoopChoice = false;
        }

        OnDialogueReplied?.Invoke(_currentLine);
    }
    else
    {
        _waitingLoopChoice = false;
    }
}

    public void SelectReply(int index)
{
    if (_currentLine == null) return;
    if (_currentLine.replyOptions == null) return;
    if (_currentLine.replyBranches == null) return;
    if (index < 0 || index >= _currentLine.replyBranches.Count) return;

    _waitingLoopChoice = false;

    DialogueData nextDialogue = _currentLine.replyBranches[index];

    _lineQueue.Clear();

    if (nextDialogue == null)
    {
        if (_currentLine.loopQuestion)
        {
            _returnLine = null;
        }

        EndDialogue();
        return;
    }

    foreach (var line in nextDialogue.lines)
    {
        _lineQueue.Enqueue(line);
    }

    DisplayNextLine();
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

        _currentLine = null;
        _returnLine = null;

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
